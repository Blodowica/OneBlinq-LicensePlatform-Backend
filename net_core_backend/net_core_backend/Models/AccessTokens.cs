using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class AccessTokens : DefaultModel
    {
        public string AccessToken { get; set; }
        public bool Active { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual int UserId { get; set; }

        public virtual Users User { get; set; }

        public AccessTokens()
        {

        }

        public AccessTokens(string accessToken, Users createdBy)
        {
            AccessToken = accessToken;
            User = createdBy;
            UserId = createdBy.Id;
        }
    }
}
