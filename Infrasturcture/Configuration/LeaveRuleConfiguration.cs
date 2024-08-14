using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class LeaveRuleConfiguration : IEntityTypeConfiguration<LeaveRule>
{
    public void Configure(EntityTypeBuilder<LeaveRule> builder)
    {
        builder
            .HasOne(lr => lr.Country)
            .WithMany(c => c.LeaveRules)
            .HasForeignKey(lr => lr.CountryId);

        builder
            .HasOne(lr => lr.Tenant)
            .WithMany(t => t.LeaveRules)
            .HasForeignKey(lr => lr.TenantId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

