using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Context
{
    public static class PrepDb
    {
        public static void PrepMigration(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            ApplyMigrations(serviceScope.ServiceProvider.GetService<IDbContextFactory<OneBlinqDBContext>>());
        }

        private static void ApplyMigrations(IDbContextFactory<OneBlinqDBContext> contextFactory)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                using var context = contextFactory.CreateDbContext();

                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }
    }
}
