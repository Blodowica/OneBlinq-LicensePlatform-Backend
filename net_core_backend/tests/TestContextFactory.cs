using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace tests
{
    public class TestContextFactory : IDbContextFactory<OneBlinqDBContext>
    {
        public DbContextOptions<OneBlinqDBContext> options { get; private set; }

        public TestContextFactory()
        {
            options = new DbContextOptionsBuilder<OneBlinqDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        public OneBlinqDBContext CreateDbContext()
        {
            return new OneBlinqDBContext(options);
        }
    }
}
