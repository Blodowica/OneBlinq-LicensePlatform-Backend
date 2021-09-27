using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class Licenses : DefaultModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public string PurchaseLocation { get; set; }
        public bool Active { get; set; } = true;
        public string GumroadID { get; set; }
        public string LicenseKey { get; set; }
        public int UserId { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<ActivationLogs> ActivationLogs { get; set; }
        public virtual ICollection<LicenseProducts> LicenseProducts { get; set; }

        public Licenses()
        {

        }

        public Licenses(Users user, DateTime expiresAt, string purchaseLocation, string gumroadID, string licenseKey)
        {
            User = user;
            UserId = user.Id;
            ExpiresAt = expiresAt;
            PurchaseLocation = purchaseLocation;
            GumroadID = gumroadID;
            LicenseKey = licenseKey;
        }
    }
}
