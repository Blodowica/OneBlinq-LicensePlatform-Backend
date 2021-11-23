using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_core_backend.Models;

namespace net_core_backend.Repository
{
    public interface ILicenseRepository:IRepositoryBase<Licenses>
    {

        Task<PaginatioPagedList<Licenses>> GetLicenses(PaginationLicenseRequest pagingParameters);

    }
}
