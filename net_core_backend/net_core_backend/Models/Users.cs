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
        [JsonIgnore]
        public string GumroadID { get; set; }

        [JsonIgnore]
        public virtual ICollection<Licenses> Licenses { get; set; }
        [JsonIgnore]
        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Users(string _email, string _firstName, string _lastName, string _password, string _gumroadID = null)
        {
            Email = _email;
            FirstName = _firstName;
            LastName = _lastName;
            Password = _password;
            GumroadID = _gumroadID;
        }
    }
}
