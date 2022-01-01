using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public int MaxUses { get; set; } = 0;
    }
}
