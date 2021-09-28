using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace net_core_backend.Models
{
    public partial class Users : DefaultModel
    {
        
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Admin { get; set; } = false;
        public virtual ICollection<Licenses> Licenses { get; set; }
        public virtual ICollection<AccessTokens> AccessTokens { get; set; }
        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }

        [JsonIgnore]
        public string GumroadID { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Users(string email, string firstName, string lastName, string password, string gumroadID = null)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            GumroadID = gumroadID;
        }
    }
}
