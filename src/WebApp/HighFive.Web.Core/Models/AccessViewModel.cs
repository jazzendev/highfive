using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HighFive.Web.Core.Models
{
    public abstract class AccessViewModel:MonitorViewModel
    {
        public DateTime? AccessStartTime { get; set; }
        public DateTime? AccessEndTime { get; set; }
        public DateTime? BlockStartTime { get; set; }
        public DateTime? BlockEndTime { get; set; }

        public bool IsAllowed { get; set; }
        public bool IsBlocked { get; set; }
    }
}
