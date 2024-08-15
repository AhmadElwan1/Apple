namespace Domain.Entities;

public class Tenant
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<LeaveRule> LeaveRules { get; set; } = new List<LeaveRule>();

    public ICollection<Unit> Units { get; set; } = new List<Unit>();
}