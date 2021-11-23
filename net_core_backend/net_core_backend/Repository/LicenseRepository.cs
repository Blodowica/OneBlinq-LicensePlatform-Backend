using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_core_backend.Models;
namespace net_core_backend.Repository
{
    public class LicenseRepository:RepositoryBase<Licenses>,ILicenseRepository
    {

        public LicenseRepository(OneBlinqDBContext repositoryContext)
            :base(repositoryContext)
        {

        }

        public Task<PaginatioPagedList<Licenses>> GetLicenses(PaginationParameters pagingParameters)
        {
            return Task.FromResult(
                PaginatioPagedList<Licenses>.GetPagedList(FindAll()
                .OrderBy(s => s.Id),
                pagingParameters.PageNumber,
                pagingParameters.PageSize));
        }

        

    }
}
