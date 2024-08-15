using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest?> GetLeaveRequestByIdAsync(int id);
        Task AddLeaveRequestAsync(LeaveRequest leaveRequest);
        Task UpdateLeaveRequestAsync(LeaveRequest leaveRequest);
        Task DeleteLeaveRequestAsync(int id);
        Task SaveChangesAsync();
        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestsAsync();
    }
}