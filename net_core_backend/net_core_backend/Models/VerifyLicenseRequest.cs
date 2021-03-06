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
        public String UniqueUserId { get; set; }

        [Required]
        public String PlatformName { get; set; }

        [Required]
        public String PluginName { get; set; }

        [Required]
        public String LicenseKey { get; set; }
    }
}
