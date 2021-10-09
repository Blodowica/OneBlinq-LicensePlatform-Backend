using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Models
{
    public class CreateAccessTokenResponse
    {
        public String AccessToken { get; set; }

        public CreateAccessTokenResponse (AccessTokens accessToken)
        {
            AccessToken = accessToken.AccessToken;
        }
    }
}
