using Microsoft.EntityFrameworkCore;
using net_core_backend.Context;
using net_core_backend.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace tests
{
    class TestContextFactory : IContextFactory
    {

        public TestContextFactory()
        {

        }
        public OneBlinqDBContext CreateDbContext(string[] smth = null)
        {
            var options = new DbContextOptionsBuilder<OneBlinqDBContext>();
            options.UseInMemoryDatabase("TestDb");

            var context = new OneBlinqDBContext(options.Options);

            //guys, check if it is a correct way to do, because tutorials say thta it is

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
    }
}
