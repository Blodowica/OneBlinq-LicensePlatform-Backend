using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IAccessTokenService
    {
        Task<CreateAccessTokenResponse> CreateAccessToken();
        Task ToggleAccessToken(int accessTokenId);
        Task CheckAccessToken(string accessToken);
    }
}
