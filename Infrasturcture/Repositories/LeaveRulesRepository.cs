using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LeaveRulesRepository : ILeaveRulesRepository
    {
        private readonly LeaveDbContext _context;

        public LeaveRulesRepository(LeaveDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LeaveRule> GetAllRules()
        {
            return _context.LeaveRules
                .Include(r => r.Country)
                .Include(r => r.Tenant)
                .ToList();
        }
    }
}