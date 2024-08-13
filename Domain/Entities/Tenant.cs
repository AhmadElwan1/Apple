using Domain.Rules;

namespace Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<LeaveRule> LeaveRules { get; set; } = new List<LeaveRule>();
}