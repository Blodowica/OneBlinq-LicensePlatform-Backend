using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadPlans
    {
        public GumroadTiers Tier { get; set; }
        public string Recurrence { get; set; }
        public int Price_Cents { get; set; }
    }
}
