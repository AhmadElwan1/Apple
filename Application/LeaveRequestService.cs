using Domain.Abstractions;
using Domain.DTOs;
using Domain.Entities;
using RulesEngine.Models;

namespace Application
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRulesRepository _leaveRulesRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private RulesEngine.RulesEngine _rulesEngine;

        public LeaveRequestService(
            ILeaveRulesRepository leaveRulesRepository,
            IEmployeeRepository employeeRepository,
            ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRulesRepository = leaveRulesRepository;
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            LoadRules();
        }

        private void LoadRules()
        {
            List<Rule> rules = _leaveRulesRepository.GetAllRules()
                .Select(r => new Rule
                {
                    RuleName = r.RuleName,
                    Expression = r.Expression,
                    SuccessEvent = r.SuccessEvent
                })
                .ToList();

            Workflow workflow = new Workflow
            {
                WorkflowName = "LeaveRequestWorkflow",
                Rules = rules
            };

            _rulesEngine = new RulesEngine.RulesEngine(new[] { workflow }, null);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        }

        public async Task<string> ApproveLeaveRequestAsync(int employeeId)
        {
            Employee? employee = await GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                return "Employee not found.";
            }

            RuleParameter employeeRuleParams = new RuleParameter("employee", employee);
            List<RuleResultTree>? results = await _rulesEngine.ExecuteAllRulesAsync("LeaveRequestWorkflow", employeeRuleParams);

            RuleResultTree? successfulRule = results.FirstOrDefault(r => r.IsSuccess);
            if (successfulRule == null)
            {
                return "Leave Not Approved";
            }

            string? successEvent = successfulRule.Rule.SuccessEvent;
            (TimeSpan duration, string message) = GetLeaveDurationAndMessage(successEvent);

            if (duration != TimeSpan.Zero)
            {
                DateTime startDate = DateTime.UtcNow;
                DateTime endDate = startDate.Add(duration);

                string creationResult = await CreateLeaveRequestAsync(employeeId, startDate, endDate, true);
                return creationResult.Contains("Employee not found") ? creationResult : message;
            }

            return "Leave Not Approved";
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

        private async Task<string> CreateLeaveRequestAsync(int employeeId, DateTime startDate, DateTime endDate, bool isApproved)
        {
            Employee? employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                return "Employee not found.";
            }

            LeaveRequest leaveRequest = new LeaveRequest
            {
                EmployeeId = employeeId,
                StartDate = startDate,
                EndDate = endDate,
                IsApproved = isApproved
            };

            await _leaveRequestRepository.AddLeaveRequestAsync(leaveRequest);

            return "Leave request created successfully.";
        }
    }
}
