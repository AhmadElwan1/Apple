using Domain.Abstractions;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly LeaveDbContext _dbContext;

        public CountryRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> CreateCountryAsync(string name)
        {
            Country country = new Country { Name = name };
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();
            return country;
        }

        public async Task<bool> ActivateCountryAsync(int id)
        {
            Country? country = await _dbContext.Countries
                .Include(c => c.LeaveRules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null || !country.LeaveRules.Any())
                return false;

            country.Status = "Active";
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<LeaveRule> AddLeaveRuleAsync(int countryId, LeaveRuleDto leaveRuleDto)
        {
            Country? country = await _dbContext.Countries
                .Include(c => c.LeaveRules)
                .FirstOrDefaultAsync(c => c.Id == countryId);

            LeaveRule rule = new LeaveRule
            {
                RuleName = leaveRuleDto.RuleName,
                Expression = leaveRuleDto.Expression,
                SuccessEvent = leaveRuleDto.SuccessEvent,
                FailureEvent = leaveRuleDto.FailureEvent,
                TenantId = leaveRuleDto.TenantId,
                CountryId = countryId
            };

            country.LeaveRules.Add(rule);
            _dbContext.LeaveRules.Add(rule);
            await _dbContext.SaveChangesAsync();

            return rule;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _dbContext.Countries.ToListAsync();
        }

        public async Task<bool> DeleteCountryAsync(int id)
        {
            Country? country = await _dbContext.Countries.FindAsync(id);
            if (country == null)
                return false;

            _dbContext.Countries.Remove(country);
            await _dbContext.SaveChangesAsync();
            return true;
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
    }
}
