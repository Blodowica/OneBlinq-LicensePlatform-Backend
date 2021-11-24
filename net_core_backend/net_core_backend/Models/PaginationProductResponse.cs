using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class PaginationProductResponse
    {

        public List<PaginationProductItem> Products { get; set; }

        public int MaxPages { get; set; }

    }
}
