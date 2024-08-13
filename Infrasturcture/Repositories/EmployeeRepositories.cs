using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly LeaveDbContext _context;

        public EmployeeRepository(LeaveDbContext context)
        {
            _context = context;
        }

        public Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return _context.Employees.FindAsync(employeeId).AsTask();
        }
    }
}