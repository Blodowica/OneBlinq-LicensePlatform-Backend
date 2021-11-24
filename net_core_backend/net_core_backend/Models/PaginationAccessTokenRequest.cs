using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class PaginationAccessTokenRequest
    {

        //Filters
        //global
        public string GlobalFilter { get; set; }

        //per column
        public int? FilterId { get; set; }
        public string FilterAccessToken { get; set; }
        public string FilterEmail { get; set; }
        public bool? FilterActive { get; set; }
        public DateTime? FilterCreatedAt { get; set; } = DateTime.UtcNow;


        //Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }


    }
}
