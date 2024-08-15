using Domain.DTOs.LeaveRule;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ICountryRepository
    {
        Task<Country> CreateCountryAsync(string name);
        Task<bool> ActivateCountryAsync(int id);
        Task<LeaveRule> AddLeaveRuleAsync(int countryId, LeaveRuleDto leaveRuleDto);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> DeleteLeaveRuleAsync(int ruleId);
    }
}