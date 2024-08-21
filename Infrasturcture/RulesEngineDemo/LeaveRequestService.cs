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

        public async Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName)
        {
            try
            {
                Employee? employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return "Employee not found.";
                }

                bool hasApprovedLeave = await _context.LeaveRequests
                    .AnyAsync(lr => lr.EmployeeId == employeeId && lr.IsApproved);

                if (hasApprovedLeave)
                {
                    return "An approved leave already exists for this employee.";
                }

                List<RuleResultTree> results = await ExecuteRulesAsync(employee, leaveTypeName);

                RuleResultTree? matchingRule = results
                    .FirstOrDefault(r => r.Rule.RuleName == leaveTypeName && r.IsSuccess);

                string approvalMessage = matchingRule?.Rule.SuccessEvent ?? "Leave Not Approved";

                (TimeSpan duration, string message) = GetLeaveDurationAndMessage(leaveTypeName);

                DateTime startDate = DateTime.UtcNow;
                DateTime endDate = startDate.Add(duration);

                await CreateLeaveRequestAsync(employee.Id, startDate, endDate, duration != TimeSpan.Zero, leaveTypeName);

                return approvalMessage;
            }
            catch (Exception ex)
            {
                return "An error occurred while processing the leave request.";
            }
        }

        private async Task<List<RuleResultTree>> ExecuteRulesAsync(Employee employee, string leaveTypeName)
        {
            Console.WriteLine($"Executing rules for leaveTypeName: {leaveTypeName}");

            return await _rulesEngine.ExecuteAllRulesAsync("LeaveRequestWorkflow", employee, leaveTypeName);
        }

        private (TimeSpan, string) GetLeaveDurationAndMessage(string? ruleName)
        {
            var leaveTypeConfigurations = new Dictionary<string, (TimeSpan, string)>
            {
                { "FatherhoodLeaveRule", (TimeSpan.FromDays(3), "Fatherhood Leave Approved for 3 days") },
                { "AnnualLeaveRuleLessThan5", (TimeSpan.FromDays(14), "Annual Leave Approved for 14 Days") },
                { "AnnualLeaveRuleMoreThan5", (TimeSpan.FromDays(21), "Annual Leave Approved for 21 Days") },
                { "SickLeaveRule", (TimeSpan.FromDays(1), "Sick Leave Approved for 1 Day") },
                { "MaternityLeaveRule", (TimeSpan.FromDays(60), "Maternity Leave Approved for 2 Months") }
            };

            if (leaveTypeConfigurations.TryGetValue(ruleName ?? string.Empty, out var result))
            {
                return result;
            }

            return (TimeSpan.Zero, "Leave Not Approved");
        }

        private async Task CreateLeaveRequestAsync(int employeeId, DateTime startDate, DateTime endDate, bool isApproved, string leaveTypeName)
        {
            if (string.IsNullOrEmpty(leaveTypeName))
            {
                throw new ArgumentException("LeaveTypeName cannot be null or empty.", nameof(leaveTypeName));
            }

            LeaveRequest leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                IsApproved = isApproved,
                LeaveTypeName = leaveTypeName
            };

            try
            {
                await _context.LeaveRequests.AddAsync(leaveRequest);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
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

            bool hasApprovedLeave = await HasApprovedLeaveAsync(employeeId);
            if (hasApprovedLeave)
            {
                return "Employee already has an approved leave request.";
            }

            List<RuleResultTree> results = await ExecuteRulesAsync(employee, leaveTypeName);

            RuleResultTree? matchingRule = results
                .FirstOrDefault(r => r.Rule.RuleName == leaveTypeName && r.IsSuccess);

            string approvalMessage = matchingRule?.Rule.SuccessEvent ?? "Leave Not Approved";

            (TimeSpan duration, string leaveMessage) = GetLeaveDurationAndMessage(leaveTypeName);

            if (duration == TimeSpan.Zero)
            {
                return "Leave Not Approved";
            }

            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.Add(duration);

            await CreateLeaveRequestAsync(employee.Id, startDate, endDate, duration != TimeSpan.Zero, leaveTypeName);

            return leaveMessage;
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
