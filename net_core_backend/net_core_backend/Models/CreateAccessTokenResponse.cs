using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateAccessTokenResponse
    {
        public int UserId { get; set; }
        public String AccessToken { get; set; }

        public CreateAccessTokenResponse (Users user, AccessTokens accessToken)
        {
            UserId = user.Id;
            AccessToken = accessToken.AccessToken;
        }
    }
}
