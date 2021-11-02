using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IGumroadService
    {
        Task RegisterLicense(GumroadSaleRequest request);
        Task DeactivateLicense(GumroadDeactivateRequest request);
        Task ReactivateLicense(GumroadReactivateRequest request);
        Task UpdateLicense(GumroadUpdateRequest request);
        Task CancelLicense(GumroadCancelRequest request);
    }
}
