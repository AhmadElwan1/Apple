using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRequestService
    {
        Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<bool> HasApprovedLeaveAsync(int employeeId);
        Task<string> CreateLeaveRequestAsync(int employeeId, string leaveTypeName);
        Task<string> GetCountryNameByIdAsync(int employeeId);
    }
}