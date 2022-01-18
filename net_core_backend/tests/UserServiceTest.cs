using net_core_backend.Models;
using net_core_backend.Services;
using Shouldly;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using tests;
using Xunit;
using BC = BCrypt.Net.BCrypt;

namespace tests
{
	public class UserServiceTest
	{
		private readonly TestContextFactory testContextFactory;
		private UserService UST;

		public UserServiceTest()
		{
			testContextFactory = new TestContextFactory();
			UST = new UserService(testContextFactory);
		}


		[Fact]
		public async Task GetUserDetailsTest()
		{
			using var db = testContextFactory.CreateDbContext();
			var user = new Users()
			{
				FirstName = "Johnny",
				LastName = "Bravo",
				Email = "johmmy@gmail.com",
				Role = "User",
				Password = BC.HashPassword("johnny'sSecurePassword"),
				
			};

			await db.AddAsync(user);
			await db.SaveChangesAsync();

			var foundUser = await UST.GetUserDetails(user.Id);
			foundUser.Id.ToString().ShouldBeSameAs(user.Id.ToString());
			foundUser.FirstName.ShouldBeSameAs(user.FirstName);
			foundUser.LastName.ShouldBeSameAs(user.LastName);
			foundUser.Email.ShouldBeSameAs(user.Email);
			foundUser.Role.ShouldBeSameAs(user.Role);
		}


		[Fact]
        public async Task EditUserTest()
        {
            using var db = testContextFactory.CreateDbContext();
            var user = new Users
            {
				FirstName = "Johnny",
				LastName = "Bravo",
				Email = "johmmy@gmail.com",
				Role = "User",
				Password = BC.HashPassword("johnny'sSecurePassword"),
			};
			await db.AddAsync(user);
			await db.SaveChangesAsync();
			var createdUser = await UST.GetUserDetails(user.Id);
			createdUser.FirstName.ShouldBeSameAs("Johnny");

			EditUserRequest editUserRequest = new EditUserRequest
			{
				FirstName = "EditFirstname",
				LastName = "EditLastname",
				Email = "newemail.gmail.com",
				Role = "User"
				
			};

			 await UST.EditUser(editUserRequest, user.Id);
			var foundUser = await UST.GetUserDetails(user.Id);
			foundUser.FirstName.ShouldBeSameAs("EditFirstname");
			foundUser.LastName.ShouldBeSameAs("EditLastname");
			foundUser.Email.ShouldBeSameAs("newemail.gmail.com");
			foundUser.Role.ShouldBeSameAs("User");
		}




		[Fact]
		public async Task EditUserIdExceptionTest()

		{
			using var db = testContextFactory.CreateDbContext();
			var user = new Users
			{
				FirstName = "Johnny",
				LastName = "Bravo",
				Email = "johmmy@gmail.com",
				Role = "User",
				Password = BC.HashPassword("johnny'sSecurePassword"),
			};

			await db.AddAsync(user);
			await db.SaveChangesAsync();
			var createdUser = await UST.GetUserDetails(user.Id);
			createdUser.FirstName.ShouldBeSameAs("Johnny");

			EditUserRequest editUserRequest = new EditUserRequest
			{
				FirstName = "EditFirstname",
				LastName = "EditLastname",
				Email = "newemail.gmail.com",
				Role = "User"
			};

			await Should.ThrowAsync<ArgumentException>(async () =>
		   {
			   await UST.EditUser(editUserRequest, -1);
		   });

		}


	}
}