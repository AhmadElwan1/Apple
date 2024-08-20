using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder
                .Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .HasMany(c => c.LeaveTypes)
                .WithOne(lt => lt.Country)
                .HasForeignKey(lt => lt.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}