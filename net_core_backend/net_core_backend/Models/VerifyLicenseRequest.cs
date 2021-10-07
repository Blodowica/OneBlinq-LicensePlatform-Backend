using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class VerifyLicenseRequest
    {
        [Required]
        public String Email { get; set; } // should ask PO about that, may be changed later
        
        [Required]
        public String AccessToken { get; set; }

        [Required]
        public String LicenseKey { get; set; }
    }
}
