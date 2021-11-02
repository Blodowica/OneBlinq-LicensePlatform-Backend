
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadCancelRequest
    {
        public string Subscription_Id { get; set; }
        public string Resource_Name { get; set; }
        public bool cancelled_by_admin { get; set; }
        public bool cancelled_by_buyer { get; set; }
        public bool cancelled_by_seller { get; set; }
        public bool cancelled_due_to_payment_failures { get; set; }
    }
}
