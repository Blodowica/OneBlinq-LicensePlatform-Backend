using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationGeneralProductItem
    {
        public PaginationGeneralProductItem()
        {
            Ids = new List<int>();
        }
        public List<int> Ids { get; set; }
        public string Product { get; set; }
        public int Licenses { get; set; }
        public int Variants { get; set; }
    }
}
