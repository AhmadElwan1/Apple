using Domain.Entities;

namespace Domain.Abstractions
{
    public interface ILeaveRulesRepository
    {
        IEnumerable<LeaveRule> GetAllRules();
    }
}