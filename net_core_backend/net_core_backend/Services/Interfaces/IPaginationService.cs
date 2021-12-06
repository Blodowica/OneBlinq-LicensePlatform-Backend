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
        Task<PaginationResponse<PaginationLicenseItem>> GetLicenses(PaginationLicenseRequest request);
        Task<PaginationResponse<PaginationUserItem>> GetUsers(PaginationUserRequest request);
        Task<PaginationResponse<PaginationProductItem>> GetProducts(PaginationProductRequest request);
        Task<PaginationResponse<PaginationAccessTokenItem>> GetAccesTokens(PaginationAccessTokenRequest request);
        Task<PaginationResponse<PaginationFreeTrialItem>> GetFreeTrails(PaginationFreeTrialRequest request);
    }
}
