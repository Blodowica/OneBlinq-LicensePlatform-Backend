using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationAccessTokenRequest : PaginationBaseRequest
    {
        //per column
        public string FilterAccessToken { get; set; }
        public string FilterEmail { get; set; }
        public bool? FilterActive { get; set; }
        public DateTime? FilterCreatedAt { get; set; } = DateTime.UtcNow;
    }
}
