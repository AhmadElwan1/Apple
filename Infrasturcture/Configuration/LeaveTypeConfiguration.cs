using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder
                .HasKey(lt => lt.LeaveTypeId);

            builder
                .Property(lt => lt.LeaveTypeName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(lt => lt.Entilement)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(lt => lt.Accural)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(lt => lt.CarryOver)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(lt => lt.Expression)
                .IsRequired()
                .HasMaxLength(500);

            builder
                .Property(lt => lt.NoticePeriod)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasOne(lt => lt.Country)
                .WithMany(c => c.LeaveTypes)
                .HasForeignKey(lt => lt.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(lt => lt.Tenant)
                .WithMany(t => t.LeaveTypes)
                .HasForeignKey(lt => lt.TenantId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasIndex(lt => lt.CountryId);

            builder
                .HasIndex(lt => lt.TenantId);
        }
    }
}
