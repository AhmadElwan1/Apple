using Domain.Rules;

namespace Domain.Abstractions
{
    public interface ILeaveRulesRepository
    {
        IEnumerable<LeaveRule> GetAllRules();
    }
}