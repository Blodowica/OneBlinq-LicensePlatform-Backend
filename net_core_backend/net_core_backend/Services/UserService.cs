﻿using Microsoft.EntityFrameworkCore;
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
        private readonly IContextFactory contextFactory;
        public UserService(IContextFactory _contextFactory) : base(_contextFactory)
        {
            contextFactory = _contextFactory;
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
                            Activations = l.ActivationLogs.Count(),
                            MaxActivations = l.Product.MaxUses
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
            }
        }
    }
}
