using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateAccessTokenResponse
    {
        public int Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public bool Admin { get; set; }
        public String AccessToken { get; set; }
        public bool Active { get; set; }

        public CreateAccessTokenResponse (Users user, AccessTokens accessToken)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Admin = user.Admin;
            AccessToken = accessToken.AccessToken;
            Active = accessToken.Active;
        }
    }
}
