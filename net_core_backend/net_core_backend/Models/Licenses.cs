using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class Licenses : DefaultModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string PurchaseLocation { get; set; }
        public bool Active { get; set; } = true;
        public string GumroadID { get; set; }
        public string LicenseKey { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<ActivationLogs> ActivationLogs { get; set; }
        public virtual ICollection<LicenseProducts> LicenseProducts { get; set; }

        public Licenses()
        {

        }

        //public Licenses(Users _user, DateTime _expiresAt, string _purchaseLocation, string _gumroadID, string _licenseKey)
        //{
        //    User = _user;
        //    CreatedAt = DateTime.UtcNow;
        //    ExpiresAt = _expiresAt;
        //    PurchaseLocation = _purchaseLocation;
        //    GumroadID = _gumroadID;
        //    LicenseKey = _licenseKey;
        //}
    }
}
