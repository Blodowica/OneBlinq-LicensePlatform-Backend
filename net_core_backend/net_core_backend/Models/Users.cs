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
        public string Role { get; set; }
        public DateTime Birthdate { get; set; }
        public String Address { get; set; }
        public String City { get; set; }
        public String PostalCode { get; set; }
        public String Country { get; set; }

        //says if you need to send abuse notifications to this (admin!) user or not
        public bool AbuseNotifications { get; set; }
        public virtual ICollection<Licenses> Licenses { get; set; }
        public virtual ICollection<AccessTokens> AccessTokens { get; set; }
        public virtual ICollection<RefreshTokens> RefreshTokens { get; set; }
        public virtual ICollection<ForgottenPasswordTokens> ForgottenPasswordTokens { get; set; }

        [JsonIgnore]
        public string GumroadID { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public Users()
        {
            Licenses = new HashSet<Licenses>();
            AccessTokens = new HashSet<AccessTokens>();
            RefreshTokens = new HashSet<RefreshTokens>();
            ForgottenPasswordTokens = new HashSet<ForgottenPasswordTokens>();
        }

        public Users(string email, string firstName, string lastName, string password, string gumroadID = null)
        {
            Licenses = new HashSet<Licenses>();
            AccessTokens = new HashSet<AccessTokens>();
            RefreshTokens = new HashSet<RefreshTokens>();
            ForgottenPasswordTokens = new HashSet<ForgottenPasswordTokens>();

            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            GumroadID = gumroadID;
        }
    }
}
