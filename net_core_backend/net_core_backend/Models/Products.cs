using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class Products : DefaultModel
    {
        public string ProductName { get; set; }
        public string VariantName { get; set; }
        public bool Active { get; set; } = true;
        public string GumroadID { get; set; }
        public int MaxUses { get; set; }

        public virtual ICollection<Licenses> Licenses { get; set; }
        public virtual ICollection<ActivateablePlugins> ActivateablePlugins { get; set; }

        public Products()
        {
            Licenses = new HashSet<Licenses>();
            ActivateablePlugins = new HashSet<ActivateablePlugins>();
        }
    }
}
