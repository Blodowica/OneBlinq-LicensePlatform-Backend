using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationBaseRequest
    {
        public string GlobalFilter { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public int? FilterId { get; set; }
    }
}
