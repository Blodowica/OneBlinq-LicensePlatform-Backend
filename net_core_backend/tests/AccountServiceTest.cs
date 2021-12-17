using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using net_core_backend.Context;
using net_core_backend.Helpers;
using net_core_backend.Models;
using net_core_backend.Services;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class AccountServiceTest
    {
        private readonly IContextFactory contextFactory;
        private AccountService sut;
        public AccountServiceTest()
        {
            // Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeTenantId = "abcd";
            context.Request.Headers["Tenant-ID"] = fakeTenantId;
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            // Mocking database
            var mockContextFactory = new Mock<IContextFactory>();
            mockContextFactory
                .Setup(f => f.CreateDbContext(null))
                .Returns(new OneBlinqDBContext(new DbContextOptionsBuilder<OneBlinqDBContext>()
                .UseInMemoryDatabase("InMemoryTest")
                .Options));

            // Mocking appSettings
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var settings = config.Get<AppSettings>();

            var mockAppSettings = Options.Create(settings);

            sut = new AccountService(mockContextFactory.Object, mockAppSettings, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task Test1()
        {
            Assert.True(true);

        }
    }
}
