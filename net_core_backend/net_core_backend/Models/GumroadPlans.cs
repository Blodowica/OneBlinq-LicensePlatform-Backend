using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadPlans
    {
        public GumroadTiers tier { get; set; }
        public string recurrence { get; set; }
        public int price_cents { get; set; }
    }
}
