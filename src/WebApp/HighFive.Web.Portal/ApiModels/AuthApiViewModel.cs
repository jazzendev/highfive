using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.ApiModels
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class UserApiViewModel
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public string TenantName { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        [JsonIgnore]
        public string Roles { get; set; }
        [JsonIgnore]
        public IEnumerable<string> RoleList { get; set; }
    }
}
