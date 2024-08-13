using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.DB
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LeaveDbContext>
    {
        public LeaveDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<LeaveDbContext> optionsBuilder = new DbContextOptionsBuilder<LeaveDbContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=Apple;Username=postgres;Password=elwan2002");

            return new LeaveDbContext(optionsBuilder.Options);
        }
    }
}