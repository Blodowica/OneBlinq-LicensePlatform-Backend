using net_core_backend.Models;
using net_core_backend.Models.GumroadRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IGumroadService
    {
        Task RegisterLicense(string accessToken, GumroadSaleRequest request);
        Task DeactivateLicense(string accessToken, GumroadDeactivateRequest request);
        Task ReactivateLicense(string accessToken, GumroadReactivateRequest request);
        Task UpdateLicense(string accessToken, GumroadUpdateRequest request);
        Task CancelLicense(string accessToken, GumroadCancelRequest request);
    }
}
