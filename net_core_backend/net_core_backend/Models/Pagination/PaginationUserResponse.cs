using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.Pagination
{
    public class PaginationUserResponse
    {

        public List<PaginationUserItem> Users { get; set; }

        public int MaxPages { get; set; }

    }
}
