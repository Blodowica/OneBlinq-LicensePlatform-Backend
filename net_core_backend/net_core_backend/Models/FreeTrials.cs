using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class FreeTrials : DefaultModel
    {
        public String FigmaUserId { get; set; }
        public String PluginName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Active { get; set; }
        public FreeTrials()
        {

        }

        public FreeTrials(int days)
        {
            Active = true;
            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddDays(days);
        }
    }
}
