using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Core.Model
{
    public class AppAccessModel<TKey> : MonitorModel<TKey>
    {
        public DateTime? AccessStartTime { get; set; }
        public DateTime? AccessEndTime { get; set; }
        public DateTime? BlockStartTime { get; set; }
        public DateTime? BlockEndTime { get; set; }
    }
}
