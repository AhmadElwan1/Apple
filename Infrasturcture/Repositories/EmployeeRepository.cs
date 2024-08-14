using Domain.Abstractions;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly LeaveDbContext _dbContext;

        public EmployeeRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _dbContext.Employees.FindAsync(employeeId);
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<LeaveRequest?> CreateLeaveRequestAsync(int employeeId, LeaveRequest leaveRequest)
        {
            Employee? employee = await GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                return null;

            leaveRequest.EmployeeId = employeeId;
            _dbContext.LeaveRequests.Add(leaveRequest);
            await _dbContext.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            Employee? employee = await _dbContext.Employees.FindAsync(employeeId);
            if (employee == null)
                return false;

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee updatedEmployee)
        {
            Employee? existingEmployee = await _dbContext.Employees.FindAsync(employeeId);
            if (existingEmployee == null)
                return false;

            existingEmployee.Name = updatedEmployee.Name;
            existingEmployee.IsMarried = updatedEmployee.IsMarried;
            existingEmployee.HasNewBorn = updatedEmployee.HasNewBorn;

            _dbContext.Employees.Update(existingEmployee);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PartialUpdateEmployeeAsync(int employeeId, UpdateEmployeeDto updateEmployeeDto)
        {
            Employee? existingEmployee = await _dbContext.Employees.FindAsync(employeeId);
            if (existingEmployee == null)
                return false;

            if (updateEmployeeDto.Name != null)
                existingEmployee.Name = updateEmployeeDto.Name;
            if (updateEmployeeDto.IsMarried.HasValue)
                existingEmployee.IsMarried = updateEmployeeDto.IsMarried.Value;
            if (updateEmployeeDto.HasNewBorn.HasValue)
                existingEmployee.HasNewBorn = updateEmployeeDto.HasNewBorn.Value;

            _dbContext.Employees.Update(existingEmployee);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
