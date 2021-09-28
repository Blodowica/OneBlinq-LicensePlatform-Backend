using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace net_core_backend.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<OneBlinqDBContext>, IContextFactory
    {

        public ContextFactory()
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            connectionString = config.GetConnectionString("SQLCONNSTR_Database");
        }

        private readonly string connectionString;
        public ContextFactory(string connectionString)
        {
            this.connectionString = connectionString;

            var options = new DbContextOptionsBuilder<OneBlinqDBContext>();
            options.UseSqlServer(connectionString);
        }

        public OneBlinqDBContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<OneBlinqDBContext>();
            options.UseSqlServer(connectionString);

            return new OneBlinqDBContext(options.Options);
        }
    }
}
