using Domain.DTOs.LeaveRule;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ITenantRepository
    {
        Task<Tenant> CreateTenantAsync(string name);
        Task<LeaveRule> AddOrUpdateLeaveRuleAsync(int tenantId, LeaveRuleDto leaveRuleDto);
        Task<bool> DeleteLeaveRuleAsync(int ruleId);
        Task<bool> DeleteTenantAsync(int tenantId);
        Task<bool> UpdateTenantNameAsync(int tenantId, string name);
    }
}