namespace Domain.Entities;

public class Unit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TenantId { get; set; }
    public Tenant Tenant { get; set; }

    public ICollection<Employee> Employees { get; set; }
}
