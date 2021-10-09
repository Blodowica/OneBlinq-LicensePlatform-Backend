using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class GumroadUpdateRequest
    {
        public string Subscription_Id { get; set; }
        public string Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string User_Id { get; set; }
        public string User_Email { get; set; }
        public DateTime Created_At { get; set; }
        public string Charge_Occurence_Count { get; set; }
        public string Recurrence { get; set; }
        public string Type { get; set; }
        public DateTime Effective_As_Of { get; set; }
        public GumroadPlans Old_Plan { get; set; }
        public GumroadPlans New_Plan { get; set; }
        public string Resource_Name { get; set; }
    }
}
