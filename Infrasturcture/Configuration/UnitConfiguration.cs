using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasMany(u => u.Employees)
            .WithOne(e => e.Unit)
            .HasForeignKey(e => e.UnitId);

        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.Units)
            .HasForeignKey(e => e.TenantId);
    }
}

