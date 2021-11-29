using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface ILicenseKeyService
    {
        Task<GetLicenseResponse> GetLicenseDetails(int licenseId);
        Task<List<GetUserLicenseResponse>> GetAllUserLicenses(int userId);
        Task VerifyLicense(VerifyLicenseRequest model);
        Task toggleLicenseState(int licenseId);
    }
}
