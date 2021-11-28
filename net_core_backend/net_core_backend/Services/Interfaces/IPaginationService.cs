using net_core_backend.Models;
using net_core_backend.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services.Interfaces
{
    public interface IPaginationService
    {
        Task<PaginationLicenseResponse> GetLicenses(PaginationLicenseRequest request);
        Task<PaginationUserResponse> GetUsers(PaginationUserRequest request);
        Task<PaginationProductResponse> GetProducts(PaginationProductRequest request);
        Task<PaginationAccessTokenResponse> GetAccesTokens(PaginationAccessTokenRequest request);
        Task<PaginationFreeTrialResponse> GetFreeTrails(PaginationFreeTrialRequest request);
    }
}
