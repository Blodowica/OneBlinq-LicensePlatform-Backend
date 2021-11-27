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
        public String PluginName { set; get; }
        [Required]
        public  String FigmaUserId { set; get; }
    }
}
