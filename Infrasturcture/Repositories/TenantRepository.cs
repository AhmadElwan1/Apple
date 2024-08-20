using Domain.Abstractions;
using Domain.Aggregates;
using Domain.DTOs.LeaveType;
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

        public async Task<LeaveType?> AddLeaveTypeAsync(int tenantId, LeaveTypeDto leaveTypeDto)
        {
            Tenant? tenant = await _dbContext.Tenants
                .Include(t => t.LeaveTypes)
                .FirstOrDefaultAsync(t => t.Id == tenantId);

            if (tenant == null)
                return null;

            Country? country = await _dbContext.Countries
                .FirstOrDefaultAsync(c => c.Id == leaveTypeDto.CountryId1);

            if (country == null)
                return null;

            LeaveType? existingLeaveType = await _dbContext.LeaveTypes
                .FirstOrDefaultAsync(l => l.TenantId == tenantId && l.LeaveTypeName == leaveTypeDto.LeaveTypeName);

            if (existingLeaveType != null)
            {
                existingLeaveType.Entilement = leaveTypeDto.Entilement;
                existingLeaveType.Accural = leaveTypeDto.Accural;
                existingLeaveType.CarryOver = leaveTypeDto.CarryOver;
                existingLeaveType.Expression = leaveTypeDto.Expression;
                existingLeaveType.NoticePeriod = leaveTypeDto.NoticePeriod;
                existingLeaveType.CountryId = leaveTypeDto.CountryId1;
                existingLeaveType.DocumentRequired = leaveTypeDto.DocumentRequired;

                _dbContext.LeaveTypes.Update(existingLeaveType);
            }
            else
            {
                LeaveType leaveType = new LeaveType
                {
                    LeaveTypeName = leaveTypeDto.LeaveTypeName,
                    Entilement = leaveTypeDto.Entilement,
                    Accural = leaveTypeDto.Accural,
                    CarryOver = leaveTypeDto.CarryOver,
                    Expression = leaveTypeDto.Expression,
                    NoticePeriod = leaveTypeDto.NoticePeriod,
                    CountryId = leaveTypeDto.CountryId1,
                    TenantId = tenantId,
                    DocumentRequired = leaveTypeDto.DocumentRequired
                };

                _dbContext.LeaveTypes.Add(leaveType);
            }

            await _dbContext.SaveChangesAsync();

            return existingLeaveType ?? await _dbContext.LeaveTypes
                .FirstOrDefaultAsync(l => l.TenantId == tenantId && l.LeaveTypeName == leaveTypeDto.LeaveTypeName);
        }


        public Task<bool> DeleteLeaveRuleAsync(int ruleId)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveType?> AddNewLeaveTypeAsync(int tenantId, LeaveTypeDto leaveTypeDto)
        {
            Tenant? tenant = await _dbContext.Tenants.FindAsync(tenantId);
            if (tenant == null)
            {
                return null;
            }

            Country? country = await _dbContext.Countries
                .FirstOrDefaultAsync(c => c.Id == leaveTypeDto.CountryId1);

            if (country == null || country.Status != "Active")
            {
                return null;
            }

            LeaveType leaveType = new LeaveType
            {
                LeaveTypeName = leaveTypeDto.LeaveTypeName,
                Entilement = leaveTypeDto.Entilement,
                Accural = leaveTypeDto.Accural,
                CarryOver = leaveTypeDto.CarryOver,
                Expression = leaveTypeDto.Expression,
                NoticePeriod = leaveTypeDto.NoticePeriod,
                CountryId = leaveTypeDto.CountryId1,
                TenantId = tenantId,
                DocumentRequired = leaveTypeDto.DocumentRequired
            };
            _dbContext.LeaveTypes.Add(leaveType);
            await _dbContext.SaveChangesAsync();

            return leaveType;
        }

        public async Task<bool> DeleteLeaveTypeAsync(int leaveTypeId)
        {
            LeaveType? leaveType = await _dbContext.LeaveTypes.FindAsync(leaveTypeId);

            if (leaveType == null)
                return false;

            _dbContext.LeaveTypes.Remove(leaveType);

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
