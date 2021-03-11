using HighFive.Core.Model;
using HighFive.Domain.Model;
using HighFive.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.ApiModels
{
    /// <summary>
    /// used in list APIs
    /// </summary>
    public class AccountApiViewModel : MonitorViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        //public string PwdHash { get; set; }
        public bool IsPwdNeedChange { get; set; }

        public string AvatarUrl { get; set; }

        public string RoleStr { get; set; }
    }

    /// <summary>
    /// used in detailed APIs
    /// </summary>
    public class AccountDetailApiViewModel : AccountApiViewModel
    {
        public string DefaultTenantId { get; set; }
        public string DefaultTenantName { get; set; }

        #region properties for selected tenant
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        /// <summary>
        /// null for start immediately
        /// </summary>
        public DateTime? AccessStartTime { get; set; }
        /// <summary>
        /// null for never stop
        /// </summary>
        public DateTime? AccessEndTime { get; set; }
        /// <summary>
        /// null for not blocked
        /// DateTime.Max for permanent block
        /// </summary>
        public DateTime? BlockStartTime { get; set; }
        /// <summary>
        /// null for not blocked
        /// DateTime.Max for permanent block
        /// </summary>
        public DateTime? BlockEndTime { get; set; }
        #endregion

        public IEnumerable<AccountRoleApiViewModel> Roles { get; set; }
    }

    public class AccountRoleUpdateApiViewModel
    {
        public IEnumerable<string> RoleIds { get; set; }
    }

    public class AccountRoleApiViewModel : MonitorViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string AccountId { get; set; }
        public string TenantId { get; set; }
    }

    public class ChangePasswordApiViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        [Required]
        public string OldPassword { get; set; }

        [StringLength(20, ErrorMessage = "密码长度至少6位。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [Required]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "核对密码")]
        [Compare("NewPassword", ErrorMessage = "两次输入的密码不相同，请核对。")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
