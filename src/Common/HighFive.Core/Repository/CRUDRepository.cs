using AutoMapper;
using Dapper;
using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace HighFive.Core.Repository
{
    public abstract class CRUDRepository<E, D> : MonitorRepository, ICRUDRepository<D> where E : class, IMonitorModel, IDatabaseModel<string> where D : class, IDto
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        protected readonly string SINGLE_QUERY_COMMAND;
        protected readonly string ALL_QUERY_COMMAND;

        public CRUDRepository(IConnectionFactory connectionFactory,
            ILogger logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, principal)
        {
            _logger = logger;
            _mapper = mapper;

            var attributes = typeof(E).GetCustomAttributes(typeof(TableAttribute), false);
            SINGLE_QUERY_COMMAND = $"SELECT TOP 1 * FROM [{(attributes[0] as TableAttribute).Name}] WHERE @id=id";
            ALL_QUERY_COMMAND = $"SELECT * FROM [{(attributes[0] as TableAttribute).Name}] WHERE @isValid IS NULL OR @isValid=IsValid";
        }

        public virtual IEnumerable<D> GetAll(bool? isValid = true)
        {
            using (var connection = GetConnection())
            {
                IEnumerable<D> dtos = connection.Query<D>(ALL_QUERY_COMMAND, new { isValid });
                return dtos;
            }
        }

        public virtual D Get(string id)
        {
            using (var connection = GetConnection())
            {
                D dto = connection.QueryFirstOrDefault<D>(SINGLE_QUERY_COMMAND, new { id });
                return dto;
            }
        }

        public virtual D Create(D dto)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    E entity = _mapper.Map<E>(dto);

                    connection.MonitorInsert(CurrentUserId, DateTime.UtcNow, entity);

                    var result = connection.QueryFirstOrDefault<D>(SINGLE_QUERY_COMMAND, new { Id = entity.Id });
                    return result;
                }
                catch (Exception e)
                {
                    _logger?.LogError(e.Message);
                    return null;
                }
            }
        }

        public virtual D Update(D dto)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    E entity = _mapper.Map<E>(dto);

                    connection.MonitorUpdate(CurrentUserId, DateTime.UtcNow, entity);

                    var result = connection.QueryFirstOrDefault<D>(SINGLE_QUERY_COMMAND, new { Id = entity.Id });
                    return result;
                }
                catch (Exception e)
                {
                    _logger?.LogError(e.Message);
                    return null;
                }
            }
        }

        public virtual bool Remove(string id)
        {
            using (var connection = GetConnection())
            {
                var entity = connection.QueryFirstOrDefault<E>(SINGLE_QUERY_COMMAND, new { Id = id });
                if (entity != null)
                {
                    AppendMonitorData(entity);
                    entity.IsValid = false;

                    try
                    {
                        connection.Update(entity);
                        return true;
                    }
                    catch (Exception e)
                    {
                        _logger?.LogError(e.Message);
                        return false;
                    }
                }
                else
                {
                    _logger?.LogWarning($"Id '{id}' not found!");
                    return false;
                }
            }
        }

        public virtual bool Delete(string id)
        {
            using (var connection = GetConnection())
            {
                var entity = connection.QueryFirstOrDefault<E>(SINGLE_QUERY_COMMAND, new { Id = id });
                if (entity != null)
                {
                    try
                    {
                        connection.Delete(entity);
                        return true;
                    }
                    catch (Exception e)
                    {
                        _logger?.LogError(e.Message);
                        return false;
                    }
                }
                else
                {
                    _logger?.LogWarning($"Id '{id}' not found!");
                    return false;
                }
            }
        }
    }
}
