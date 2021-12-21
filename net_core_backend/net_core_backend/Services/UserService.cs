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
    public class UserService : DataService<DefaultModel>, IUserService
    {
        private readonly IDbContextFactory<OneBlinqDBContext> contextFactory;
        public UserService(IDbContextFactory<OneBlinqDBContext> _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
        }

        public async Task EditUser(EditUserRequest request, int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    throw new ArgumentException("No user found with given id");
                }
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Role = request.Role;

                db.Update(user);
                await db.SaveChangesAsync();
            }
        }

        public async Task<GetUserResponse> GetUserDetails(int userId)
        {
            using (var db = contextFactory.CreateDbContext())
            {
                return await db.Users
                    .Include(u => u.Licenses)
                        .ThenInclude(l => l.Product)
                    .Include(u => u.Licenses)
                        .ThenInclude(l => l.ActivationLogs)
                    .Where(u => u.Id == userId)
                    .Select(u => new GetUserResponse 
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Role = u.Role,
                        Licenses = u.Licenses.Select(l => new GetUserResponse.UserLicense
                        {
                            LicenseKey = l.LicenseKey,
                            ProductName = l.Product.ProductName,
                            Activations = l.ActivationLogs
                                .Where(a => a.Successful)
                                .Select(a => a.UniqueUserId)
                                .Distinct()
                                .Count(),
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
            }
        }
    }
}
