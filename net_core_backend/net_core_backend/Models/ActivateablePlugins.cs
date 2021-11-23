using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class ActivateablePlugins : DefaultModel
    {
        public string Plugin { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
    }
}
