using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationFreeTrialResponse
    {

        public List<PaginationFreeTrialsItem> FreeTrials { get; set; }

        public int MaxPages { get; set; }

    }
}
