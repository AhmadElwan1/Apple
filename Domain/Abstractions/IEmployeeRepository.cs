using Domain.DTOs.Employee;
using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<bool> UpdateEmployeeAsync(int employeeId, Employee updatedEmployee);
        Task<bool> PartialUpdateEmployeeAsync(int employeeId, UpdateEmployeeDto updateEmployeeDto);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    }
}