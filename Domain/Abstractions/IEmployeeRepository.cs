using Domain.Entities;

namespace Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeByIdAsync(int employeeId);
    }
}
