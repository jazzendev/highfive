using System;
using HighFive.Extensions.Dapper.Contrib;

namespace HighFive.Core.Model
{
    public class MonitorModel<TKey> : IMonitorModel<TKey>
    {
        [ExplicitKey]
        public TKey Id { get; set; }

        [DbReadonly]
        public string CreatorId { get; set; }
        [DbReadonly]
        public DateTime CreationTime { get; set; }

        public string EditorId { get; set; }
        public DateTime? LastEditTime { get; set; }

        public bool IsValid { get; set; }
    }
}
