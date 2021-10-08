using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class VerifyLicenseResponse
    {
        public int UserId { get; set; }
        public string LicenseKey { get; set; }

        public VerifyLicenseResponse(Users user, Licenses license)
        {
            UserId = user.Id;
            LicenseKey = license.LicenseKey;
        }
    }
}
