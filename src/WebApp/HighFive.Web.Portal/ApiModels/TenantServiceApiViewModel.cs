using HighFive.Core.Utility;
using HighFive.Domain;
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
    public class TenantServiceApiViewModel : AccessViewModel
    {
        public string TenantId { get; set; }
        public string ServiceCode { get; set; }
        [JsonIgnore]
        public AppServiceCode AppServiceCode { get; set; }
        public string ServiceCodeName { get { return this.AppServiceCode.Name(); } }
        public string ServiceCodeDescription { get { return this.AppServiceCode.Description(); } }
    }
}
