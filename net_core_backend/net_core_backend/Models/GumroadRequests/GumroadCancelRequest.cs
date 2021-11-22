
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace net_core_backend.Models.GumroadRequests
{
    public class GumroadCancelRequest
    {
        public string subscription_id { get; set; }
        public string resource_name { get; set; }
        public string cancelled_by_admin { get; set; }
        public string cancelled_by_buyer { get; set; }
        public string cancelled_by_seller { get; set; }
        public string cancelled_due_to_payment_failures { get; set; }
    }
}
