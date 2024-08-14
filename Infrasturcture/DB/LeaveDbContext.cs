using Domain.DTOs;
using Domain.Entities;
using Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    public class LeaveDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LeaveRule> LeaveRules { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<LeaveRequest>()
                .HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LeaveRule>()
                .HasOne(lr => lr.Country)
                .WithMany(c => c.LeaveRules)
                .HasForeignKey(lr => lr.CountryId);

            modelBuilder.Entity<LeaveRule>()
                .HasOne(lr => lr.Tenant)
                .WithMany(t => t.LeaveRules)
                .HasForeignKey(lr => lr.TenantId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}