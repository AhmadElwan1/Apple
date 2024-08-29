using Domain.Abstractions;
using Domain.DTOs.LeaveType;
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
                .Include(c => c.LeaveTypes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null || !country.LeaveTypes.Any())
                return false;

            country.Status = "Active";
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<LeaveType> AddLeaveTypeAsync(int countryId, LeaveTypeDto leaveTypeDto)
        {
            Country? country = await _dbContext.Countries
                .FirstOrDefaultAsync(c => c.Id == countryId);

            if (country == null)
                return null;

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

        public Task<bool> DeleteLeaveRuleAsync(int ruleId)
        {
            throw new NotImplementedException();
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

    }
}
