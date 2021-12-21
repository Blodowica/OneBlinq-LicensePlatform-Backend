using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    // made it a separate model as it could be extended with other "notifications"
    public class UserNotificationsRequest
    {
        public bool AbuseNotifications { get; set; }
    }
}
