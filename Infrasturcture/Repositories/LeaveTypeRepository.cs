using Domain.Aggregates;
using Domain.Abstractions;
using Domain.DTOs.LeaveType;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly LeaveDbContext _dbContext;

        public LeaveTypeRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeaveType> AddLeaveTypeAsync(int countryId, LeaveTypeDto leaveTypeDto)
        {
            LeaveType leaveType = new LeaveType
            {
                LeaveTypeName = leaveTypeDto.LeaveTypeName,
                Entilement = leaveTypeDto.Entilement,
                Accural = leaveTypeDto.Accural,
                CarryOver = leaveTypeDto.CarryOver,
                Expression = leaveTypeDto.Expression,
                NoticePeriod = leaveTypeDto.NoticePeriod,
                CountryId = countryId,
                TenantId = leaveTypeDto.TenantId,
                DocumentRequired = leaveTypeDto.DocumentRequired
            };

            _dbContext.LeaveTypes.Add(leaveType);
            await _dbContext.SaveChangesAsync();

            return leaveType;
        }

        public async Task<LeaveType?> GetLeaveTypeByIdAsync(int leaveTypeId)
        {
            return await _dbContext.LeaveTypes.FirstOrDefaultAsync(l => l.LeaveTypeId == leaveTypeId);
        }

        public async Task<LeaveType?> UpdateLeaveTypeAsync(int leaveTypeId, LeaveTypeDto leaveTypeDto)
        {
            LeaveType? leaveType = await _dbContext.LeaveTypes.FirstOrDefaultAsync(l => l.LeaveTypeId == leaveTypeId);

            if (leaveType == null)
                return null;

            leaveType.LeaveTypeName = leaveTypeDto.LeaveTypeName;
            leaveType.Entilement = leaveTypeDto.Entilement;
            leaveType.Accural = leaveTypeDto.Accural;
            leaveType.CarryOver = leaveTypeDto.CarryOver;
            leaveType.Expression = leaveTypeDto.Expression;
            leaveType.NoticePeriod = leaveTypeDto.NoticePeriod;
            leaveType.CountryId = leaveTypeDto.CountryId1;
            leaveType.TenantId = leaveTypeDto.TenantId;
            leaveType.DocumentRequired = leaveTypeDto.DocumentRequired;

            _dbContext.LeaveTypes.Update(leaveType);
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

        public async Task<IEnumerable<LeaveType>> GetAllLeaveTypesAsync()
        {
            return await _dbContext.LeaveTypes.ToListAsync();
        }
    }
}
