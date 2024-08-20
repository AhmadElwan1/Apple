using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder
                .HasMany(t => t.LeaveTypes)
                .WithOne(lt => lt.Tenant)
                .HasForeignKey(lt => lt.TenantId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
