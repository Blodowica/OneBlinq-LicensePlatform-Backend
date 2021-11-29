using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Helpers
{
    public class AppSettings
    {
        public string ProductionFrontendUrl { get; set; }
        public string NoReplyEmail { get; set; }
        public string NoReplyEmailPassword { get; set; }
        public string Secret { get; set; }
        public string GumroadAccessToken { get; set; }
    }
}
