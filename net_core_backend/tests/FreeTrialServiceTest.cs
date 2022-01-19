using Microsoft.EntityFrameworkCore;
using net_core_backend.Models;
using net_core_backend.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class FreeTrialServiceTest
    {
        private readonly IDbContextFactory<OneBlinqDBContext> testContextFactory;
        private FreeTrialService sut;

        public FreeTrialServiceTest()
        {
            // Mocking database
            testContextFactory = new TestContextFactory();

            sut = new FreeTrialService(testContextFactory);
        }

        [Fact]
        public async Task CreateFreeTrialTest()
        {
            FreeTrialRequest request = new FreeTrialRequest()
            {
                PlatformName = "Figma",
                PluginName = "Forms",
                UniqueUserId = "userId1"
            };

            Should.NotThrow(sut.CreateFreeTrial(request));

            var db = testContextFactory.CreateDbContext();

            var uniqueUser= await db.UniqueUsers
                .Include(us => us.FreeTrials)
                .FirstOrDefaultAsync(
                us => us.ExternalUserServiceId == request.UniqueUserId && us.ExternalServiceName == request.PlatformName);

            var freeTrial = await db.FreeTrials.FirstOrDefaultAsync(ft => ft.PluginName == request.PluginName);

            uniqueUser.FreeTrials.ShouldContain(freeTrial);
        }

        [Fact]
        public async Task VerifyFreeTrialTest()
        {
            FreeTrialRequest request = new FreeTrialRequest()
            {
                PlatformName = "Figma",
                PluginName = "Forms",
                UniqueUserId = "userId1"
            };

            var db = testContextFactory.CreateDbContext();

            var user = new UniqueUsers()
            {
                ExternalServiceName = request.PlatformName,
                ExternalUserServiceId = request.UniqueUserId,
            };
            await db.UniqueUsers.AddAsync(user);

            var freeTrial = new FreeTrials()
            {
                UniqueUser = user,
                PluginName = request.PluginName,
                EndDate = DateTime.Now.AddDays(14),
            };

            await db.FreeTrials.AddAsync(freeTrial);
            await db.SaveChangesAsync();

            Should.NotThrow(sut.VerifyFreeTrial(request));
        }

        [Fact]
        public async Task SetEndDateTest()
        {
            var freeTrial = new FreeTrials()
            {
                PluginName = "Figma",
                EndDate = DateTime.Now,
            };

            var db = testContextFactory.CreateDbContext();

            await db.FreeTrials.AddAsync(freeTrial);
            await db.SaveChangesAsync();

            var dbFreeTrial = await db.FreeTrials.FirstOrDefaultAsync(ft => ft.PluginName == freeTrial.PluginName);
            DateTime newEndDate = DateTime.Now.AddDays(14);

            Should.NotThrow(sut.SetEndDate(dbFreeTrial.Id, newEndDate));

            db.Entry(dbFreeTrial).Reload();

            dbFreeTrial.EndDate.ShouldBe(newEndDate);
        }

        [Fact]
        public async Task ToggleFreeTriaTest()
        {
            var freeTrial = new FreeTrials()
            {
                PluginName = "Figma",
                EndDate = DateTime.Now.AddDays(14),
            };

            var db = testContextFactory.CreateDbContext();

            await db.FreeTrials.AddAsync(freeTrial);
            await db.SaveChangesAsync();

            var dbFreeTrial = await db.FreeTrials.FirstOrDefaultAsync(ft => ft.PluginName == freeTrial.PluginName);

            dbFreeTrial.Active.ShouldBeTrue();

            await sut.ToggleFreeTrial(dbFreeTrial.Id);

            db.Entry(dbFreeTrial).Reload();

            dbFreeTrial.Active.ShouldBeFalse();
        }

    }
}
