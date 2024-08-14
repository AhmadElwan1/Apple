using Domain.DTOs;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<LeaveRequest?> CreateLeaveRequestAsync(int employeeId, LeaveRequest leaveRequest);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<bool> UpdateEmployeeAsync(int employeeId, Employee updatedEmployee);
        Task<bool> PartialUpdateEmployeeAsync(int employeeId, UpdateEmployeeDto updateEmployeeDto);
    }
}