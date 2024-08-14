public class LeaveRuleDto
{
    public string RuleName { get; set; }
    public string Expression { get; set; }
    public string SuccessEvent { get; set; }
    public string FailureEvent { get; set; }
    public int? TenantId { get; set; }
}