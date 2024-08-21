using Domain.Abstractions;
using Domain.DTOs.Unit;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly LeaveDbContext _dbContext;

        public UnitRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit?> GetUnitByIdAsync(int unitId)
        {
            Unit? unit = await _dbContext.Units
                .FirstOrDefaultAsync(u => u.Id == unitId);

            return unit;
        }

        public async Task<Unit> CreateUnitAsync(CreateUnitDto createUnitDto)
        {
            Unit unit = new Unit
            {
                Name = createUnitDto.Name,
                TenantId = createUnitDto.TenantId
            };

            _dbContext.Units.Add(unit);
            await _dbContext.SaveChangesAsync();
            return unit;
        }

        public async Task<bool> UpdateUnitAsync(int unitId, UpdateUnitDto updateUnitDto)
        {
            Unit? unit = await _dbContext.Units.FindAsync(unitId);
            if (unit == null) return false;

            if (updateUnitDto.Name != null)
                unit.Name = updateUnitDto.Name;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUnitAsync(int unitId)
        {
            Unit? unit = await _dbContext.Units.FindAsync(unitId);
            if (unit == null) return false;

            _dbContext.Units.Remove(unit);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsByTenantIdAsync(int tenantId)
        {
            return await _dbContext.Units
                .Where(u => u.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await _dbContext.Units.ToListAsync();
        }
    }
}
