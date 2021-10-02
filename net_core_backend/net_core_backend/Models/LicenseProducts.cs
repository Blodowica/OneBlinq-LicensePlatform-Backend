using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class LicenseProducts : DefaultModel
    {
        public virtual Licenses License { get; set; }
        public virtual Products Product { get; set; }

        public int LicenseId { get; set; }
        public int ProductId { get; set; }

        public LicenseProducts()
        {

        }

        public LicenseProducts(Products product)
        {
            Product = product;
            ProductId = product.Id;
        }
    }
}
