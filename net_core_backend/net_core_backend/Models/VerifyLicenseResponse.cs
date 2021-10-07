using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class VerifyLicenseResponse
    {
        public int Id { get; set; } // do we need Id if already have email?
        public bool Admin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LicenseKey { get; set; }
        public bool LicenseActive { get; set; }
        public string AccessToken { get; set; }
        public bool AccessTokenActive { get; set; }

        public VerifyLicenseResponse(Users user, Licenses license, AccessTokens accessToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Admin = user.Admin;
            LicenseKey = license.LicenseKey;
            LicenseActive = license.Active;
            AccessToken = accessToken.AccessToken;
            AccessTokenActive = accessToken.Active;
        }
    }
}
