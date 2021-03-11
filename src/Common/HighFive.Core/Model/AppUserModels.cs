using HighFive.Extensions.Dapper.Contrib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Model
{
    /// <summary>
    /// lower role & permission could not access high level
    /// </summary>
    public enum AppAuthLevel
    {
        NA = 0,
        /// <summary>
        /// Default Role = SuperAdmin
        /// </summary>
        SuperAdmin = 1,
        /// <summary>
        /// Default Role = SuperOperator
        /// </summary>
        SuperOperation = 10,
        /// <summary>
        /// Default Role = TenantAdmin
        /// </summary>
        TenantAdmin = 100,
        /// <summary>
        /// Default Role = TenantOperation
        /// </summary>
        TenantOperation = 1000
    }

    public class AppUser : MonitorModel<string>
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string PwdHash { get; set; }
    }

    public class AppRole : MonitorModel<string>
    {
        public string Name { get; set; }
        [DbReadonly]
        public AppAuthLevel AuthLevel { get; set; }
    }

    public class AppPermission : MonitorModel<string>
    {
        public string Name { get; set; }
        [DbReadonly]
        public AppAuthLevel AuthLevel { get; set; }
    }
}
