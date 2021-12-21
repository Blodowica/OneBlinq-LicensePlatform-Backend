using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationResponse<T>
    {
        public List<T> Records { get; set; }

        public int MaxPages { get; set; }
    }
}
