using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRequestService
    {
        Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
    }
}
