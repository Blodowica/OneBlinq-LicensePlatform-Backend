using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class Products : DefaultModel
    {
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public bool Active { get; set; } = true;
        public string Recurrance { get; set; }
        public string GumroadID { get; set; }

        public virtual ICollection<LicenseProducts> LicenseProducts { get; set; }

        public Products()
        {

        }

        //public Products(string _productName, int _price, string _currency, string _recurrance, string _gumroadID)
        //{
        //    ProductName = _productName;
        //    Price = _price;
        //    Currency = _currency;
        //    Recurrance = _recurrance;
        //    GumroadID = _gumroadID;
        //}
    }
}
