using Domain.DTOs.Unit;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IUnitRepository
    {
        Task<Unit?> GetUnitByIdAsync(int unitId);
        Task<Unit> CreateUnitAsync(CreateUnitDto createUnitDto);
        Task<bool> UpdateUnitAsync(int unitId, UpdateUnitDto updateUnitDto);
        Task<bool> DeleteUnitAsync(int unitId);
        Task<IEnumerable<Unit>> GetAllUnitsByTenantIdAsync(int tenantId);
        Task<IEnumerable<Unit>> GetAllUnitsAsync();

    }
}
