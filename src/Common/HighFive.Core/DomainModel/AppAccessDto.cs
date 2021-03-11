using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.DomainModel
{
    public class AppAccessDto : MonitorDto
    {
        public DateTime? AccessStartTime { get; set; }
        public DateTime? AccessEndTime { get; set; }
        public DateTime? BlockStartTime { get; set; }
        public DateTime? BlockEndTime { get; set; }

        public bool IsAllowed
        {
            get
            {
                var timestamp = DateTime.UtcNow;

                if (!AccessStartTime.HasValue && !AccessEndTime.HasValue)
                {
                    return false;
                }

                if (!AccessStartTime.HasValue)
                {
                    AccessStartTime = DateTime.MinValue;
                }
                if (!AccessEndTime.HasValue)
                {
                    AccessEndTime = DateTime.MaxValue;
                }

                return timestamp >= AccessStartTime && timestamp <= AccessEndTime;
            }
        }

        public bool IsBlocked
        {
            get
            {
                var timestamp = DateTime.UtcNow;

                if (!BlockStartTime.HasValue && !BlockEndTime.HasValue)
                {
                    return false;
                }

                if (!BlockStartTime.HasValue)
                {
                    BlockStartTime = DateTime.MinValue;
                }
                if (!BlockEndTime.HasValue)
                {
                    BlockEndTime = DateTime.MaxValue;
                }

                return timestamp >= BlockStartTime && timestamp <= BlockEndTime;
            }
        }
    }
}
