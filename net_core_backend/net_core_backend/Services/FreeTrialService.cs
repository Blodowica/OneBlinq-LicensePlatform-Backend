using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Services
{
    public class FreeTrialService : DataService<DefaultModel>, IFreeTrialService
    {
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        public FreeTrialService(IDbContextFactory<OneBlinqDBContext> _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        // giving a free trial for a plugin
        public async Task CreateFreeTrial(FreeTrialRequest model)
        {
            using var db = contextFactory.CreateDbContext();
            
            var freeTrialState = await CheckFreeTrial(model.PluginName, model.UniqueUserId, model.PlatformName);
            if (freeTrialState == FreeTrialStates.EXPIRED)
            {
                throw new ArgumentException("A free trial was activated earlier and has already expired");
            }
            else if (freeTrialState == FreeTrialStates.RUNNING)
            {
                throw new ArgumentException("A free trial was already activated and is still active");
            }

            var uniqueUser = await db.UniqueUsers.FirstOrDefaultAsync(u => u.ExternalUserServiceId == model.UniqueUserId && u.ExternalServiceName == model.PlatformName);
            if (uniqueUser == null)
            {
                uniqueUser = new UniqueUsers
                {
                    ExternalUserServiceId = model.UniqueUserId,
                    ExternalServiceName = model.PlatformName
                };

                db.Add(uniqueUser);
                await db.SaveChangesAsync();
            }
            // creating a free trial with length of 1 day
            FreeTrials freeTrial = new FreeTrials(14); // (!) check with PO how long a free trial should be
            freeTrial.UniqueUserId = uniqueUser.Id;
            freeTrial.PluginName = model.PluginName;

            await db.AddAsync(freeTrial);
            await db.SaveChangesAsync();
        }

        public async Task VerifyFreeTrial(FreeTrialRequest model)
        {
            var freeTrialState = await CheckFreeTrial(model.PluginName, model.UniqueUserId, model.PlatformName);
            if (freeTrialState == FreeTrialStates.EXPIRED)
            {
                throw new ArgumentException("The free trial has already expired");
            }
            else if (freeTrialState == FreeTrialStates.DO_NOT_EXIST)
            {
                throw new ArgumentException("A free trial was not created yet");
            }
        }

        public async Task<FreeTrialStates> CheckFreeTrial(string pluginName, string uniqueUserId, string platformName)
        {
            using var db = contextFactory.CreateDbContext();
            var freeTrial = await db.FreeTrials
                .Include(ft => ft.UniqueUser)
                .Where(
                    ft => ft.PluginName == pluginName && 
                    ft.UniqueUser.ExternalUserServiceId == uniqueUserId && 
                    ft.UniqueUser.ExternalServiceName == platformName)
                .FirstOrDefaultAsync();

            if (freeTrial != null)
            {
                return freeTrial.Active ? FreeTrialStates.RUNNING : FreeTrialStates.EXPIRED;
            }

            return FreeTrialStates.DO_NOT_EXIST;
        }

        public async Task SetEndDate(int freeTrialId, DateTime newEndDate)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var freeTrial = db.FreeTrials.FirstOrDefault(ft => ft.Id == freeTrialId);
                freeTrial.EndDate = newEndDate;

                db.Update(freeTrial);
                await db.SaveChangesAsync();
            }
        }

        public async Task ToggleFreeTrial(int freeTrialId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var freeTrial = await db.FreeTrials.FirstOrDefaultAsync(ft => ft.Id == freeTrialId);
                freeTrial.EndDate = freeTrial.Active ? DateTime.UtcNow.AddMinutes(-1) : DateTime.UtcNow.AddDays(14) ;

                db.Update(freeTrial);
                await db.SaveChangesAsync();
            }
        }
    }
}
