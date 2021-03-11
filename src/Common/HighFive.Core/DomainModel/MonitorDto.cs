using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.DomainModel
{
    public class MonitorDto : IDto
    {
        public string Id { get; set; }

        public string CreatorId { get; set; }
        public string EditorId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastEditTime { get; set; }
        public bool IsValid { get; set; }
    }
}
