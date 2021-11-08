using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GetLicenseResponse
    {
        public GetLicenseResponse()
        {
            ActivationLogs = new List<ACLogs>();
        }
        public int LicenseId { get; set; }
        public string LicenseKey { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public string Recurrence { get; set; }
        public string PurchaseLocation { get; set; }
        public string EndedReason { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public int MaxUses { get; set; }
        public int Activations { get; set; }
        public bool Active { get; set; }
        public List<ACLogs> ActivationLogs { get; set; }

        public class ACLogs
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public bool Successful { get; set; }
        }
    }
}
