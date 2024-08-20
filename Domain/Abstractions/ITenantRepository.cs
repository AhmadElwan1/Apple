using Domain.Aggregates;
using Domain.DTOs.LeaveType;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ITenantRepository
    {
        Task<Tenant> CreateTenantAsync(string name);
        Task<LeaveType> AddLeaveTypeAsync(int tenantId, LeaveTypeDto leaveRuleDto);
        Task<bool> DeleteLeaveRuleAsync(int ruleId);
        Task<bool> DeleteTenantAsync(int tenantId);
        Task<bool> UpdateTenantNameAsync(int tenantId, string name);
    }
}