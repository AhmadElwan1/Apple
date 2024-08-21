using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;

namespace Infrastructure.RulesEngineDemo
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly LeaveDbContext _context;
        private readonly RulesEngine.RulesEngine _rulesEngine;

        public LeaveRequestService(LeaveDbContext dbContext, RulesEngine.RulesEngine rulesEngine)
        {
            _context = dbContext;
            _rulesEngine = rulesEngine;
        }

        public async Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName, string country)
        {
            try
            {
                Employee? employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return "Employee not found.";
                }

                List<RuleResultTree> results = await ExecuteRulesAsync(employee, leaveTypeName, country);
                RuleResultTree? successfulRule = results.FirstOrDefault(r => r.IsSuccess);
                string approvalMessage = successfulRule?.Rule.SuccessEvent ?? "Leave Not Approved";

                (TimeSpan duration, string message) = GetLeaveDurationAndMessage(leaveTypeName, country);

                DateTime startDate = DateTime.UtcNow;
                DateTime endDate = startDate.Add(duration);

                await CreateLeaveRequestAsync(employee.Id, startDate, endDate, duration != TimeSpan.Zero);

                return message;
            }
            catch (Exception ex)
            {
                // Log exception
                return "An error occurred while processing the leave request.";
            }
        }

        private async Task<List<RuleResultTree>> ExecuteRulesAsync(Employee employee, string leaveTypeName, string country)
        {
            RuleParameter employeeRuleParams = new RuleParameter("employee", employee);
            RuleParameter leaveTypeParam = new RuleParameter("leaveTypeName", leaveTypeName);
            RuleParameter countryParam = new RuleParameter("country", country);

            return await _rulesEngine.ExecuteAllRulesAsync("LeaveRequestWorkflow", employeeRuleParams, leaveTypeParam, countryParam);
        }

        private (TimeSpan, string) GetLeaveDurationAndMessage(string leaveTypeName, string country)
        {
            var leaveTypeConfigurations = new Dictionary<string, Dictionary<string, (TimeSpan, string)>>
            {
                // Configuration omitted for brevity
            };

            if (leaveTypeConfigurations.TryGetValue(country, out var leaveTypes) &&
                leaveTypes.TryGetValue(leaveTypeName, out var result))
            {
                return result;
            }

            return (TimeSpan.Zero, "Leave Not Approved");
        }

        private async Task CreateLeaveRequestAsync(int employeeId, DateTime startDate, DateTime endDate, bool isApproved)
        {
            var leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                IsApproved = isApproved
            };

            try
            {
                await _context.LeaveRequests.AddAsync(leaveRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception
                throw new ApplicationException("Failed to create leave request.", ex);
            }
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees.FindAsync(employeeId);
        }

        public async Task<bool> HasApprovedLeaveAsync(int employeeId)
        {
            return await _context.LeaveRequests.AnyAsync(lr => lr.EmployeeId == employeeId && lr.IsApproved);
        }

        public async Task<string> CreateLeaveRequestAsync(int employeeId, string leaveTypeName)
        {
            Employee? employee = await GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                return "Employee not found.";
            }

            string country = await GetCountryNameByIdAsync(employee.CountryId);

            List<RuleResultTree> results = await ExecuteRulesAsync(employee, leaveTypeName, country);
            RuleResultTree? successfulRule = results.FirstOrDefault(r => r.IsSuccess);
            string approvalMessage = successfulRule?.Rule.SuccessEvent ?? "Leave Not Approved";

            (TimeSpan duration, string message) = GetLeaveDurationAndMessage(leaveTypeName, country);

            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.Add(duration);

            await CreateLeaveRequestAsync(employee.Id, startDate, endDate, duration != TimeSpan.Zero);

            return message;
        }

        public async Task<string> GetCountryNameByIdAsync(int countryId)
        {
            return await _context.Countries
                .Where(c => c.Id == countryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync() ?? "Unknown";
        }
    }
}
