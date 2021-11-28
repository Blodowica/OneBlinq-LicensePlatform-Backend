using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationFreeTrialRequest
    {

        //Filters
        //global
        public string GlobalFilter { get; set; }

        //per column
        public int? FilterId { get; set; }
        public string FilterFigmaId { get; set; }
        public string FilterPluginName { get; set; }
        public DateTime? FilterStartDate { get; set; }
        public DateTime? FilterEndDate { get; set; }
        public bool? FilterActive { get; set; }


        //Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }

    }
}
