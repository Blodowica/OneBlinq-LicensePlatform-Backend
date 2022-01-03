using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateUniqueIdResponse
    {
        public string UniqueId { get; set; }

        public CreateUniqueIdResponse(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}
