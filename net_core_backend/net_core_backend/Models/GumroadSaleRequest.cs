
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
        public string Currency { get; set; }
        public string Email { get; set; }
        public string Ip_Country { get; set; }
        public string License_Key { get; set; }
        public int Order_Number { get; set; }
        public string Permalink { get; set; }
        public float Price { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Purchaser_Id { get; set; }
        public string Quantity { get; set; }
        public string Recurrence { get; set; }
        public string Sale_Id { get; set; }
        public string Sale_Timestamp { get; set; }
        public string Subscription_Id { get; set; }
        public ProductVariants Variants { get; set; }
        public string Resource_Name { get; set; }
    }
}
