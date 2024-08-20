using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasOne<Country>()
            .WithMany()
            .HasForeignKey(e => e.CountryId);

        builder.HasOne<Unit>()
            .WithMany()
            .HasForeignKey(e => e.UnitId);
    }

}
