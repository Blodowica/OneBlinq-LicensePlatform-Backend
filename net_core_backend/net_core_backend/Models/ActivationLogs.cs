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
        public string FigmaUserId { get; set; }

        public String Message { get; set; }

        // In case we want only the owner to be able use their licenses,
        // Then we have to keep track of the user id/email as well

        // Do we need access tokens in there?

        public ActivationLogs()
        {

        }

        public ActivationLogs(bool successful)
        {
            CreatedAt = DateTime.UtcNow;
            Successful = successful;
        }
    }
}
