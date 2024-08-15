using Domain.Abstractions;
using Domain.DTOs.LeaveRule;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly LeaveDbContext _dbContext;

        public TenantRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tenant> CreateTenantAsync(string name)
        {
            Tenant tenant = new Tenant { Name = name };
            _dbContext.Tenants.Add(tenant);
            await _dbContext.SaveChangesAsync();
            return tenant;
        }

        public async Task<LeaveRule> AddOrUpdateLeaveRuleAsync(int tenantId, LeaveRuleDto leaveRuleDto)
        {
            Tenant? tenant = await _dbContext.Tenants.FindAsync(tenantId);
            if (tenant == null)
                return null;

            Country? country = await _dbContext.Countries
                .Include(c => c.LeaveRules)
                .FirstOrDefaultAsync(c => c.Id == leaveRuleDto.CountryId && c.LeaveRules.Any(lr => lr.TenantId == tenantId));

            if (country == null || country.Status != "Active")
                return null;

            LeaveRule? rule = await _dbContext.LeaveRules
                .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.RuleName == leaveRuleDto.RuleName);

            if (rule != null)
            {
                rule.Expression = leaveRuleDto.Expression;
                rule.SuccessEvent = leaveRuleDto.SuccessEvent;
                rule.FailureEvent = leaveRuleDto.FailureEvent;
                rule.CountryId = leaveRuleDto.CountryId;
            }
            else
            {
                rule = new LeaveRule
                {
                    RuleName = leaveRuleDto.RuleName,
                    Expression = leaveRuleDto.Expression,
                    SuccessEvent = leaveRuleDto.SuccessEvent,
                    FailureEvent = leaveRuleDto.FailureEvent,
                    TenantId = tenantId,
                    CountryId = leaveRuleDto.CountryId
                };
                _dbContext.LeaveRules.Add(rule);
            }

            await _dbContext.SaveChangesAsync();
            return rule;
        }

        public async Task<bool> DeleteLeaveRuleAsync(int ruleId)
        {
            LeaveRule? rule = await _dbContext.LeaveRules.FindAsync(ruleId);
            if (rule == null)
                return false;

            _dbContext.LeaveRules.Remove(rule);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTenantAsync(int tenantId)
        {
            Tenant? tenant = await _dbContext.Tenants.FindAsync(tenantId);
            if (tenant == null)
                return false;

            _dbContext.Tenants.Remove(tenant);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTenantNameAsync(int tenantId, string newName)
        {
            Tenant? tenant = await _dbContext.Tenants.FindAsync(tenantId);
            if (tenant == null)
                return false;

            tenant.Name = newName;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
