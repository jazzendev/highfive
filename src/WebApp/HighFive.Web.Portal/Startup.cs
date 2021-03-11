using AutoMapper;
using HighFive.Core.Configuration;
using HighFive.Core.Provider;
using HighFive.Core.Repository;
using HighFive.Core.Security;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using HighFive.Domain.Repository;
using HighFive.Web.Core.Models;
using HighFive.Web.Core.Policies;
using HighFive.Web.Portal.ApiModels;
using HighFive.Web.Portal.Authorization;
using HighFive.Web.Portal.Error;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HighFive.Web.Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenSection = Configuration.GetSection("Token");
            var tokenconfig = tokenSection.Get<TokenConfig>();
            services.Configure<TokenConfig>(tokenSection);
            services.Configure<DefaultValueConfig>(Configuration.GetSection("DefaultValue"));
            services.Configure<ApiDefaultValueConfig>(Configuration.GetSection("ApiDefaultValue"));
            services.Configure<DbConnectionStringConfig>(Configuration.GetSection("ConnectionString:Database"));
            services.Configure<AzureStorageConnectionStringConfig>(Configuration.GetSection("ConnectionString:AzureStorage"));

            //services.AddControllersWithViews();
            services.AddControllers(options => options.Filters.Add(new UnauthorizedHttpExceptionFilter()))
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new ObjectResult(new ApiResultModel<HttpStatusCode>
                        {
                            Data = HttpStatusCode.BadRequest,
                            Message = "×Ö¶ÎÑéÖ¤Ê§°Ü¡£",
                            Error = new ApiError()
                            {
                                Code = "invalid",
                                Message = "×Ö¶ÎÑéÖ¤Ê§°Ü¡£",
                                Field = string.Join('|', context.ModelState.Keys),
                                Resource = string.Join('|', context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                            }
                        });
                        result.StatusCode = (int)HttpStatusCode.BadRequest;
                        return result;
                    };
                });

            // API JWT authentication
            // !IMPORTANT, TO MAKE RSA WORK, MUST CONFIG AZURE FIRST!
            // https://stackoverflow.com/questions/46114264/x509certificate2-on-azure-app-services-azure-websites-since-mid-2017
            JwtProvider jwtProvider = new JwtProvider(tokenconfig);
            services.AddSingleton<IJwtProvider>(jwtProvider);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SecurityTokenValidators.Add(jwtProvider.Validator);
                    options.EventsType = typeof(JwtBearerEvents);
                });

            services.AddSingleton<IAuthorizationPolicyProvider, AppPermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionPolicyHandler>();

            // initiate core
            services.AddSingleton(ConfigureMapper());
            services.AddSingleton(new SimplePasswordHasher());
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<JwtBearerEvents, AppJwtBearerEvents>();

            // inject repositories            
            services.AddScoped<IAppAccessRepository, AppAccessRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<ITenantMgmtRepository, TenantMgmtRepository>(); 
            services.AddScoped<ITenantServiceRepository, TenantServiceRepository>();

            // inject principal for specific classes
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePagesWithReExecute("/api/error/{0}");

            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapFallbackToFile("index.html");
            });
        }

        //private Task HandleApiFallback(HttpContext context)
        //{
        //    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
        //    {
        //        var result = new ObjectResult(new ApiResultModel<HttpStatusCode>
        //        {
        //            Data = HttpStatusCode.BadRequest,
        //            Message = "×Ö¶ÎÑéÖ¤Ê§°Ü¡£",
        //            Error = new ApiError()
        //            {
        //                Code = "invalid",
        //                Message = "×Ö¶ÎÑéÖ¤Ê§°Ü¡£",
        //                Field = string.Join('|', context.ModelState.Keys),
        //                Resource = string.Join('|', context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
        //            }
        //        });
        //        result.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Task.FromResult(result);
        //    }
        //}

        public IMapper ConfigureMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // db to dto
                cfg.CreateMap<Account, AccountDto>().ReverseMap();
                cfg.CreateMap<TenantAccount, AccountDto>().ReverseMap();
                cfg.CreateMap<Tenant, TenantDto>().ReverseMap();
                cfg.CreateMap<TenantService, TenantServiceDto>().ReverseMap();
                cfg.CreateMap<Role, RoleDto>().ReverseMap();
                cfg.CreateMap<Permission, PermissionDto>().ReverseMap();
                cfg.CreateMap<RolePermission, RolePermissionDto>().ReverseMap();

                //dto to view model
                // nothing will be here

                // api view model
                cfg.CreateMap<AccountDto, UserApiViewModel>().ReverseMap();
                cfg.CreateMap<AccountDto, AccountApiViewModel>().ReverseMap();
                cfg.CreateMap<AccountDto, AccountDetailApiViewModel>().ReverseMap();
                cfg.CreateMap<AccountDisplayDto, AccountApiViewModel>().ReverseMap();
                cfg.CreateMap<TenantDto, TenantApiViewModel>().ReverseMap();
                cfg.CreateMap<TenantServiceDto, TenantServiceApiViewModel>().ReverseMap();
                cfg.CreateMap<AccountRoleDto, AccountRoleApiViewModel>().ReverseMap();
                cfg.CreateMap<RoleDto, RoleApiViewModel>().ReverseMap();
                cfg.CreateMap<PermissionDto, PermissionApiViewModel>().ReverseMap();
                cfg.CreateMap<RolePermissionDto, SelectedPermissionApiViewModel>().ReverseMap();
                cfg.CreateMap<RolePermissionDto, RolePermissionApiViewModel>().ReverseMap();

            });
            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
