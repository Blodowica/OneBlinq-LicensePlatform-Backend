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
using Shouldly;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace tests
{
    public class AccountServiceTest
    {
        private readonly IDbContextFactory<OneBlinqDBContext> testContextFactory;
        private AccountService sut;
        public AccountServiceTest()
        {
            // Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            };

            mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims).Returns(claims);

            // Mocking database
            testContextFactory = new TestContextFactory();

            // Mocking appSettings
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            var settings = config.Get<AppSettings>();

            var mockAppSettings = Options.Create(settings);

            // Mocking IMailingService 
            var mockMailingService = new Mock<MailingService>(mockAppSettings);

            sut = new AccountService(testContextFactory, mockAppSettings, mockHttpContextAccessor.Object, mockMailingService.Object);
        }

        [Fact]
        public async Task CreateAdminTest()
        {
            AddUserRequest model = new AddUserRequest
            {
                FirstName = "First",
                LastName = "Last",
                Email = "Email",
                Password = "password"

            };
            //await sut.CreateAdmin(model);
            Should.NotThrow(sut.CreateAdmin(model));

            var db = testContextFactory.CreateDbContext();

            var user = await db.Users.FirstOrDefaultAsync((u => u.Email == model.Email));

            Assert.NotNull(user);
            Should.Equals(user.Role, "Admin");
        }

        [Fact]
        public async Task ChangePasswordTest()
        {
            ChangePasswordRequest model = new ChangePasswordRequest
            {
                CurrentPassword = "Current password",
                NewPassword = "New passworD"
            };

            var db = testContextFactory.CreateDbContext();
            var user = new Users
            {
                Email = "email",
                Password = BC.HashPassword("Current password")
            };
            await db.Users.AddAsync(user);  
            db.SaveChanges();

            await sut.ChangePassword(model);

            // Reload method updates the database you are using with the up-to-date data
            db.Entry(user).Reload(); 

            var user2 = await db.Users.FirstOrDefaultAsync((u => u.Email == user.Email));

            BC.Verify(model.NewPassword, user2.Password).ShouldBeTrue();
        }
    }
}
