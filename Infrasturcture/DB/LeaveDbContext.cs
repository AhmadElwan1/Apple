using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    public class LeaveDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Employee> Employees { get; set;}
        public DbSet<Country> Countries { get; set; }
        public DbSet<LeaveRule> LeaveRules { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}