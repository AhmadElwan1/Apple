namespace Domain.Entities;

public class LeaveRule
{
    public int Id { get; set; }
    public string RuleName { get; set; }
    public string Expression { get; set; }
    public string SuccessEvent { get; set; }
    public string FailureEvent { get; set; }
    public int CountryId { get; set; }
    public Country Country { get; set; }
    public int? TenantId { get; set; }
    public Tenant Tenant { get; set; }
}