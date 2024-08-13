using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRequestService
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<string> ApproveLeaveRequestAsync(int employeeId);
    }
}