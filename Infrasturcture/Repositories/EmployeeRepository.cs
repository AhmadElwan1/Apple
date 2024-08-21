using Domain.Abstractions;
using Domain.DTOs.Employee;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

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
            existingEmployee.HasNewBorn = updatedEmployee.HasNewBorn;
            existingEmployee.YearsOfService = updatedEmployee.YearsOfService;
            existingEmployee.UnitId = updatedEmployee.UnitId;
            existingEmployee.CountryId = updatedEmployee.CountryId;
            existingEmployee.Gender = updatedEmployee.Gender;
            existingEmployee.MaritalStatus = updatedEmployee.MaritalStatus;
            existingEmployee.Religion = updatedEmployee.Religion;

            _dbContext.Employees.Update(existingEmployee);

            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> PartialUpdateEmployeeAsync(int employeeId, UpdateEmployeeDto updateEmployeeDto)
        {
            Employee? existingEmployee = await _dbContext.Employees.FindAsync(employeeId);
            if (existingEmployee == null)
                return false;

            if (!string.IsNullOrEmpty(updateEmployeeDto.Name))
                existingEmployee.Name = updateEmployeeDto.Name;

            if (updateEmployeeDto.HasNewBorn.HasValue)
                existingEmployee.HasNewBorn = updateEmployeeDto.HasNewBorn.Value;

            if (updateEmployeeDto.YearsOfService.HasValue)
                existingEmployee.YearsOfService = updateEmployeeDto.YearsOfService.Value;

            if (updateEmployeeDto.Gender.HasValue)
                existingEmployee.Gender = updateEmployeeDto.Gender.Value;

            if (updateEmployeeDto.MaritalStatus.HasValue)
                existingEmployee.MaritalStatus = updateEmployeeDto.MaritalStatus.Value;

            if (updateEmployeeDto.Religion.HasValue)
                existingEmployee.Religion = updateEmployeeDto.Religion.Value;

            _dbContext.Employees.Update(existingEmployee);
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }
    }
}
