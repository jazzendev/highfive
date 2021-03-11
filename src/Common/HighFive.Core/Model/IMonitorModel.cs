using HighFive.Extensions.Dapper.Contrib;
using System;

namespace HighFive.Core.Model
{
    public interface IMonitorModel<TKey> : IDatabaseModel<TKey>, IMonitorModel
    {
    }

    public interface IMonitorModel
    {
        [DbReadonly]
        public string CreatorId { get; set; }
        [DbReadonly]
        public DateTime CreationTime { get; set; }        
        public string EditorId { get; set; }
        public DateTime? LastEditTime { get; set; }
    }

    public static class IMonitorExtension
    {
        public static IMonitorModel AppendMonitorData(this IMonitorModel monitor, string userId, DateTime? timestamp = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("UserId could not be null.");
            }
            if (string.IsNullOrEmpty(monitor.CreatorId))
            {
                monitor.CreatorId = userId;
                monitor.CreationTime = timestamp.HasValue ? timestamp.Value : DateTime.UtcNow;
            }
            else
            {
                monitor.EditorId = userId;
                monitor.LastEditTime = timestamp.HasValue ? timestamp.Value : DateTime.UtcNow;
            }
            return monitor;
        }
    }
}
