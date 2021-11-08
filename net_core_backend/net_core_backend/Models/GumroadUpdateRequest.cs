using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadUpdateRequest
    {
        public string subscription_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string user_id { get; set; }
        public string user_email { get; set; }
        public string created_at { get; set; }
        public string charge_occurence_count { get; set; }
        public string recurrence { get; set; }
        public string type { get; set; }
        public string effective_as_of { get; set; }
        public GumroadPlans new_plan { get; set; }
        public string resource_name { get; set; }
    }
}
