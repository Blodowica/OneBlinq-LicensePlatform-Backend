using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationFreeTrialRequest : PaginationBaseRequest
    {
        //per column
        public string FilterUniqueUserId { get; set; }
        public string FilterPlatform { get; set; }
        public string FilterPluginName { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public bool? FilterActive { get; set; }
    }
}
