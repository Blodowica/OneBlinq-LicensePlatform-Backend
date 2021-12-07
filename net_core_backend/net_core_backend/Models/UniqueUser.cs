using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class UniqueUser
    {
        public UniqueUser()
        {
            ActivationLogs = new HashSet<ActivationLogs>();
        }

        public int Id { get; set; }
        public string ExternalUserServiceId { get; set; }
        public string ExternalServiceName { get; set; }

        public virtual ICollection<ActivationLogs> ActivationLogs { get; set; }
    }
}
