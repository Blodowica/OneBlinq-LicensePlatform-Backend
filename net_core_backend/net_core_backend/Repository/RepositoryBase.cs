using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace net_core_backend.Repository
{
    public class RepositoryBase<T>:IRepositoryBase<T> where T : class 
    {
        protected OneBlinqDBContext RepositoryContext { get; set; }


        public RepositoryBase(OneBlinqDBContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

    }
}
