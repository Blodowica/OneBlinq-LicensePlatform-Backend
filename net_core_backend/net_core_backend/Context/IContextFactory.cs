using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Context
{
    public interface IContextFactory
    {
        public OneBlinqDBContext CreateDbContext(string[] args = null);
    }
}
