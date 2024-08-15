using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
{
    public void Configure(EntityTypeBuilder<LeaveRequest> builder)
    {
        builder
            .HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRequests)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}