using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRequestService
    {
        Task<string> ApproveLeaveRequestAsync(Employee employee);
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<bool> HasApprovedLeaveAsync(int employeeId);

    }
}
