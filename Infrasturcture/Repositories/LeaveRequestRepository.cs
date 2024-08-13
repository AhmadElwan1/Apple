using Domain.Abstractions;
using Domain.DTOs;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly LeaveDbContext _dbContext;

        public LeaveRequestRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id)
        {
            return await _dbContext.LeaveRequests.FindAsync(id);
        }

        public async Task AddLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _dbContext.LeaveRequests.Add(leaveRequest);
            await SaveChangesAsync();
        }

        public async Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            _dbContext.LeaveRequests.Update(leaveRequest);
            await SaveChangesAsync();
        }

        public async Task DeleteLeaveRequestAsync(int id)
        {
            var leaveRequest = await _dbContext.LeaveRequests.FindAsync(id);
            if (leaveRequest != null)
            {
                _dbContext.LeaveRequests.Remove(leaveRequest);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}