using Domain.DTOs.LeaveType;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ICountryRepository
    {
        Task<Country> CreateCountryAsync(string name);
        Task<bool> ActivateCountryAsync(int id);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<LeaveType> AddLeaveTypeAsync(int countryId, LeaveTypeDto leaveTypeDto);
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> DeleteLeaveTypeAsync(int ruleId);
    }
}