using Domain.Entities;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using RulesEngine.Models;

namespace Infrastructure.RulesEngineDemo
{
    public class LeaveRequestService
    {
        private readonly LeaveDbContext _context;
        private readonly RulesEngine.RulesEngine _rulesEngine;

        public LeaveRequestService(LeaveDbContext context, RulesEngine.RulesEngine rulesEngine)
        {
            _context = context;
            _rulesEngine = rulesEngine;
        }

        public async Task<LeaveRequest> SubmitLeaveRequestAsync(LeaveRequest leaveRequest)
        {
            // Save the leave request to the database
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            // Fetch the LeaveType using the LeaveTypeName from the request
            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(lt => lt.Name == leaveRequest.LeaveTypeName);

            if (leaveType == null)
            {
                throw new ArgumentException("Invalid LeaveTypeName");
            }

            // Call ApproveLeaveRequestAsync to handle approval logic
            string approvalMessage = await ApproveLeaveRequestAsync(leaveRequest.EmployeeId, leaveType.Id);

            // Update the leave request with approval status based on the approvalMessage
            leaveRequest.IsApproved = approvalMessage == "Leave Approved";

            // Save the updated leave request
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();

            // Return the updated leave request
            return leaveRequest;
        }

        private async Task<string> ApproveLeaveRequestAsync(int employeeId, int leaveTypeId)
        {
            // Fetch the LeaveType
            LeaveType? leaveType = await _context.LeaveTypes.FindAsync(leaveTypeId);
            if (leaveType == null)
            {
                return "LeaveType not found.";
            }

            // Fetch the employee
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

            // Evaluate condition asynchronously
            bool isConditionMet = await EvaluateConditionAsync(leaveType.Condition, employee);

            // Determine approval message based on condition evaluation
            return isConditionMet ? "Leave Approved" : "Leave Not Approved";
        }

        private async Task<bool> EvaluateConditionAsync(string? condition, Employee employee)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                return false;
            }

            RuleParameter ruleParameter = new RuleParameter("employee", employee);
            List<RuleResultTree> result = await _rulesEngine.ExecuteAllRulesAsync(condition, ruleParameter);
            return result.Any(r => r.IsSuccess);
        }
    }
}
