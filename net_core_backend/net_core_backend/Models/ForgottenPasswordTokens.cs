using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class ForgottenPasswordTokens : DefaultModel
    {
        public string Token { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public virtual Users User { get; set; }
    }
}
