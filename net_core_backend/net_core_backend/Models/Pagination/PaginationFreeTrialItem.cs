using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationFreeTrialItem
    {
        public int Id { get; set; }
        public string UniqueUserId { get; set; }
        public string Platform { get; set; }
        public string PluginName { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; }

    }
}
