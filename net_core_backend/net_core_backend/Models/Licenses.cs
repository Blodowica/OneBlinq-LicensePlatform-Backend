using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class Licenses : DefaultModel
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public string PurchaseLocation { get; set; }
        public bool Active { get; set; } = true;
        public string GumroadSaleID { get; set; }
        public string GumroadSubscriptionID { get; set; }
        public string LicenseKey { get; set; }
        public string Recurrence { get; set; }
        public string Currency { get; set; }
        public float Price { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public virtual Products Product { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<ActivationLogs> ActivationLogs { get; set; }

        public Licenses()
        {
            ActivationLogs = new HashSet<ActivationLogs>();
        }
    }
}
