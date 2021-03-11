using HighFive.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.ApiModels
{
    public class TenantApiViewModel : MonitorViewModel
    {
        public string RootId { get; set; }
        public string ParentId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Domain { get; set; }
    }
}
