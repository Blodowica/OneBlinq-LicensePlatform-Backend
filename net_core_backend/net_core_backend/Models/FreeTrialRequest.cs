using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class FreeTrialRequest
    {
        [Required]
        public string PluginName { set; get; }
        [Required]
        public string UniqueUserId { get; set; }
        [Required]
        public string PlatformName { get; set; }
    }
}
