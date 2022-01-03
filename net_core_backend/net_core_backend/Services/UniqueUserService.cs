using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net_core_backend.Services.Extensions;

namespace net_core_backend.Services
{
    public class UniqueUserService : DataService<DefaultModel>, IUniqueUserService
    {
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        private readonly Random random;
        public UniqueUserService(IDbContextFactory<OneBlinqDBContext> _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            random = new Random();
        }

        public async Task<CreateUniqueIdResponse> CreateId()
        {
            using (var db = contextFactory.CreateDbContext())
            {
                string randomId = null;
                while (randomId == null || await db.UniqueUsers.FirstOrDefaultAsync(u => u.ExternalUserServiceId == randomId) != null)
                {
                    randomId = Guid.NewGuid().ToString("N");
                }
                return new CreateUniqueIdResponse(randomId);
            }
        }
    }
}
