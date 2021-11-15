using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models.GumroadRequests
{
    public class GumroadProductRequest
    {
        public bool success { get; set; }
        public Product product { get; set; }

        public class Monthly
        {
            public int price_cents { get; set; }
            public object suggested_price_cents { get; set; }
        }

        public class Yearly
        {
            public int price_cents { get; set; }
            public object suggested_price_cents { get; set; }
        }

        public class RecurrencePrices
        {
            public Monthly monthly { get; set; }
            public Yearly yearly { get; set; }
        }

        public class Option
        {
            public string name { get; set; }
            public int price_difference { get; set; }
            public bool is_pay_what_you_want { get; set; }
            public RecurrencePrices recurrence_prices { get; set; }
            public object url { get; set; }
        }

        public class Variant
        {
            public string title { get; set; }
            public List<Option> options { get; set; }
        }

        public class Product
        {
            public string name { get; set; }
            public object preview_url { get; set; }
            public string description { get; set; }
            public bool customizable_price { get; set; }
            public bool require_shipping { get; set; }
            public string custom_receipt { get; set; }
            public object custom_permalink { get; set; }
            public string subscription_duration { get; set; }
            public string id { get; set; }
            public object url { get; set; }
            public int price { get; set; }
            public string currency { get; set; }
            public string short_url { get; set; }
            public object thumbnail_url { get; set; }
            public List<string> tags { get; set; }
            public string formatted_price { get; set; }
            public bool published { get; set; }
            public bool shown_on_profile { get; set; }
            public object max_purchase_count { get; set; }
            public bool deleted { get; set; }
            public List<object> custom_fields { get; set; }
            public string custom_summary { get; set; }
            public bool is_tiered_membership { get; set; }
            public List<string> recurrences { get; set; }
            public List<Variant> variants { get; set; }
            public int sales_count { get; set; }
            public double sales_usd_cents { get; set; }
        }
    }
}
