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
        Task CreateLicense(string purchaseLocation, string currency, string recurrence, int userId, int price, int productId);
        Task VerifyLicense(VerifyLicenseRequest model);
        Task ToggleLicenseState(int licenseId);
    }
}
