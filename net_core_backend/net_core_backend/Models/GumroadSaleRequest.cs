
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadSaleRequest
    {
        public string seller_id { get; set; }
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string permalink { get; set; }
        public string product_permalink { get; set; }
        public string short_product_id { get; set; }
        public string email { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string sale_id { get; set; }
        public string sale_timestamp { get; set; }
        public string purchaser_id { get; set; }
        public string subscription_id { get; set; }
        public string variants { get; set; }
        public string license_key { get; set; }
        public string ip_country { get; set; }
        public string recurrence { get; set; }
        public string resource_name { get; set; }
    }
}
