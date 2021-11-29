using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class ChangePasswordRequest
    {
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
    }
}
