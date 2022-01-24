using Microsoft.EntityFrameworkCore;
using Moq;
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
    public class LoggingServiceTest
    {
        private readonly IDbContextFactory<OneBlinqDBContext> testContextFactory;
        private LoggingService sut;
        public LoggingServiceTest()
        {
            // Mocking database
            testContextFactory = new TestContextFactory();

            // Mocking IMailingService 
            var mockMailingService = new Mock<IMailingService>();

            sut = new LoggingService(testContextFactory, mockMailingService.Object);
        }

        // This test breaks because of different behavior between in memory and production db
        //[Fact]
        //public async Task AddActivationLogTest()
        //{
        //    String licenseKey = "SuperSecretLicenseKey";

        //    var db = testContextFactory.CreateDbContext();

        //    bool successful = true;
        //    String ExternalUniqueUserId = "SuperUniqueUserId";
        //    String platformName = "SuperAwesomePlatformName";
        //    String message = "The user managed to use their license super successfully";

        //    await sut.AddActivationLog(licenseKey, successful, ExternalUniqueUserId, platformName, message);

        //    var dbLicense = await db.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == licenseKey);

        //    var activationLog = await db.ActivationLogs.FirstOrDefaultAsync(al => al.Message == message);

        //    activationLog.Message.ShouldBe(message);
        //}

        [Fact]
        public async Task RemoveUniqueUserIdLogsTest()
        {
            String externalUserServiceId = "someId";

            var uniqueUser = new UniqueUsers()
            {
                ExternalUserServiceId = externalUserServiceId
            };

            String licenseKey = "somekey";

            var license = new Licenses()
            {
                LicenseKey = licenseKey,
            };

            var actLog1 = new ActivationLogs()
            {
                UniqueUser = uniqueUser,
                License = license,
                Message = "Some cool message1"
            };

            var actLog2 = new ActivationLogs()
            {
                UniqueUser = uniqueUser,
                License = license,
                Message = "Some cool message2"
            };

            var db = testContextFactory.CreateDbContext();

            await db.UniqueUsers.AddAsync(uniqueUser);
            await db.Licenses.AddAsync(license);
            await db.SaveChangesAsync();
            await db.ActivationLogs.AddAsync(actLog1);
            await db.ActivationLogs.AddAsync(actLog2);
            await db.SaveChangesAsync();

            var dbUniqueUser = await db.UniqueUsers.FirstOrDefaultAsync(us => us.ExternalUserServiceId == uniqueUser.ExternalUserServiceId);
            var dbLicense = await db.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == licenseKey);

            await sut.RemoveUniqueUserIdLogs(dbUniqueUser.Id, dbLicense.Id);

            var dbActLog1 = await db.ActivationLogs.FirstOrDefaultAsync(al => al.Message == actLog1.Message);
            var dbActLog2 = await db.ActivationLogs.FirstOrDefaultAsync(al => al.Message == actLog2.Message);

            dbActLog1.ShouldBeNull();
            dbActLog2.ShouldBeNull();
        }
    }
}
