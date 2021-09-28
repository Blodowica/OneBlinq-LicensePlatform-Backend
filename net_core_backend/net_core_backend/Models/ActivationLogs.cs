using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class ActivationLogs : DefaultModel
    {
        public DateTime CreatedAt { get; set; }
        public bool Successful { get; set; }
        public virtual Licenses License { get; set; }

        public int LicenseId { get; set; }
        //ADD user logging data here (e.g. IPadress, MacAdress, etc.)

        public ActivationLogs()
        {

        }

        public ActivationLogs(bool successful, Licenses license)
        {
            CreatedAt = DateTime.UtcNow;
            Successful = successful;
            License = license;
            LicenseId = license.Id;
        }
    }
}
