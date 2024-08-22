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
            // Fetch employee
            Employee? employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return "Employee not found.";
            }

            // Check for existing approved leave
            bool hasApprovedLeave = await _context.LeaveRequests
                .AnyAsync(lr => lr.EmployeeId == employeeId && lr.IsApproved);

            if (hasApprovedLeave)
            {
                return "An approved leave already exists for this employee.";
            }

            // Execute rules
            List<RuleResultTree> results = await ExecuteRulesAsync(employee);

            // Determine if the leave is approved based on rule results
            RuleResultTree? successfulRule = results.FirstOrDefault(r => r.Rule.RuleName == leaveTypeName && r.IsSuccess);
            string approvalMessage = successfulRule != null ? successfulRule.Rule.SuccessEvent : "Leave Not Approved";

            // Get leave duration and message
            (TimeSpan duration, string message) = GetLeaveDurationAndMessage(approvalMessage);

            // Create leave request regardless of approval status
            DateTime startDate = DateTime.UtcNow;
            DateTime endDate = startDate.Add(duration);
            await CreateLeaveRequestAsync(employeeId, startDate, endDate, successfulRule != null && successfulRule.IsSuccess, leaveTypeName);

            return message;
        }

        private async Task<List<RuleResultTree>> ExecuteRulesAsync(Employee employee)
        {
            RuleParameter employeeRuleParams = new RuleParameter("employee", employee);
            return await _rulesEngine.ExecuteAllRulesAsync("LeaveRequestWorkflow", employeeRuleParams);
        }

        private (TimeSpan, string) GetLeaveDurationAndMessage(string successEvent)
        {
            Dictionary<string, (TimeSpan, string)> leaveDurations = new Dictionary<string, (TimeSpan, string)>
            {
                { "Fatherhood Leave Approved for 3 days", (TimeSpan.FromDays(3), "Leave Approved for 3 Days") },
                { "Annual Leave Approved for 14 Days", (TimeSpan.FromDays(14), "Leave Approved for 14 Days") },
                { "Annual Leave Approved for 21 Days", (TimeSpan.FromDays(21), "Leave Approved for 21 Days") },
                { "Sick Leave Approved for 1 Day", (TimeSpan.FromDays(1), "Leave Approved for 1 Day") },
                { "Maternity Leave Approved for 2 Months", (TimeSpan.FromDays(60), "Leave Approved for 2 Months") }
            };

            return leaveDurations.TryGetValue(successEvent, out (TimeSpan, string) result) ? result : (TimeSpan.Zero, "Leave Not Approved");
        }

        private async Task CreateLeaveRequestAsync(int employeeId, DateTime startDate, DateTime endDate, bool isApproved, string leaveTypeName)
        {
            LeaveRequest leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                IsApproved = isApproved,
                LeaveTypeName = leaveTypeName
            };

            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees.FindAsync(employeeId);
        }
    }
}
