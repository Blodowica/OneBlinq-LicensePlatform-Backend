using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class RefreshTokens
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RevokedAt { get; set; }
        public bool Active { get; set; } = true;
        public int UserId { get; set; }

        public virtual Users User { get; set; }

        public RefreshTokens()
        {

        }

        //public RefreshTokens(Users _user, string _token, DateTime _expiresAt)
        //{
        //    User = _user;
        //    Token = _token;
        //    ExpiresAt = _expiresAt;
        //    CreatedAt = DateTime.UtcNow;
        //}
    }
}
