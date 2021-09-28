using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime RevokedAt { get; set; }
        public bool Active { get; set; } = true;
        public int UserId { get; set; }

        public virtual Users User { get; set; }

        [NotMapped]
        public bool Expired => DateTime.UtcNow >= ExpiresAt;

        [NotMapped]
        public bool Useable => RevokedAt == null && !Expired;

        public RefreshTokens()
        {

        }

        public RefreshTokens(Users user, string token, DateTime expiresAt)
        {
            User = user;
            UserId = user.Id;
            Token = token;
            ExpiresAt = expiresAt;
        }
    }
}
