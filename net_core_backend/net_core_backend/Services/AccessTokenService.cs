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

namespace net_core_backend.Services
{
    public class AccessTokenService : DataService<DefaultModel>, IAccessTokenService
    {
        private readonly IContextFactory contextFactory;
        private readonly IHttpContextAccessor httpContext;
        private readonly AppSettings appSettings;
        public AccessTokenService(IContextFactory _contextFactory, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContext) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
            this.httpContext = httpContext;
            this.appSettings = appSettings.Value;
        }

        public async Task<CreateAccessTokenResponse> CreateAccessToken(CreateAccessTokenRequest requestInfo)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                //var someUser = await db.Users.FirstOrDefaultAsync(u => u.Id == 2);

                //var product = new Products { ProductName = "Whatever"};
                //product.Recurrance = "monthly";

                //await db.AddAsync(product);
                //await db.SaveChangesAsync();

                //var license = new Licenses();
                //license.LicenseKey = "666";
                //license.LicenseProducts = null;
                //someUser.Licenses.Add(license);

                //db.Update(someUser);
                //await db.SaveChangesAsync();

                //Console.WriteLine("GOT IT" + " --------------- " + db.Licenses.ToList().Count.ToString());
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email.Equals(requestInfo.Email));

                if (user == null)
                {
                    throw new ArgumentException("A user with these credentials does not exist");
                }
                if (!user.Admin)
                {
                    throw new ArgumentException("Only Admins can create access tokens");
                }

                String accessToken = GenerateAccessToken();
                foreach (var existantAccessToken in db.AccessTokens.ToList())
                {
                    if (existantAccessToken.AccessToken.Equals(accessToken))
                    {
                        accessToken = GenerateAccessToken();
                    }
                }

                var newAccessToken = new AccessTokens(accessToken, user);

                user.AccessTokens.Add(newAccessToken);
                db.Update(user);
                await db.SaveChangesAsync();

                return new CreateAccessTokenResponse(user, newAccessToken);
            }
        }

        private String GenerateAccessToken()
        {
            String accessToken = "";
            for (; accessToken.Length < 16; )
            {
                Random random1 = new Random();
                Random random2 = new Random();
                double numberOrLetter = random1.NextDouble();
                if (numberOrLetter <= 0.3)
                {
                    accessToken += random2.Next(1, 10).ToString();
                }
                else
                {
                    Random random3 = new Random();
                    double letterCase = random1.NextDouble();
                    if (letterCase < 0.5)
                    {
                        accessToken += ((char)(random2.Next(65, 91)));
                    }
                    else
                    {
                        accessToken += ((char)(random2.Next(97, 123)));
                    }
                }
            }
            return accessToken;
        }
    }
}
