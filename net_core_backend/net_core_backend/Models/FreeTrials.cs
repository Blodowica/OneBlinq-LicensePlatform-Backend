using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class FreeTrials : DefaultModel
    {
        public string FigmaUserId { get; set; }
        public string PluginName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [NotMapped]
        public bool Active => (EndDate == null || EndDate >= DateTime.UtcNow) ? true : false;

        public FreeTrials()
        {

        }

        public FreeTrials(int days)
        {
            StartDate = DateTime.UtcNow;
            EndDate = StartDate.AddDays(days);
        }
    }
}
