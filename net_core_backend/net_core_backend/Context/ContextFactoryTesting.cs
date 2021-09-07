using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using net_core_backend.Models;


namespace net_core_backend.Context
{
    public class ContextFactoryTesting : IDesignTimeDbContextFactory<OneBlinqDBContext>, IContextFactory
    {
        private readonly string connectionString;

        public ContextFactoryTesting(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public OneBlinqDBContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<OneBlinqDBContext>();
            options.UseInMemoryDatabase("TestingDatabase");

            return new OneBlinqDBContext(options.Options);
        }
    }
}
