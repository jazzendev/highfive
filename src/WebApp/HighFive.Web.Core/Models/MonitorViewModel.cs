using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HighFive.Web.Core.Models
{
    public abstract class MonitorViewModel
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string CreatorId { get; set; }
        [JsonIgnore]
        public string EditorId { get; set; }
        [JsonIgnore]
        public DateTime CreationTime { get; set; }
        [JsonIgnore]
        public DateTime? LastEditTime { get; set; }

        public bool IsValid { get; set; }
    }
}
