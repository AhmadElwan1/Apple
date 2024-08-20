namespace Domain.DTOs.LeaveType;

public class LeaveTypeDto
{
    public string LeaveTypeName { get; set; }
    public string Entilement { get; set; }
    public string Accural { get; set; }
    public string CarryOver { get; set; }
    public string Expression { get; set; }
    public string NoticePeriod { get; set; }
    public int CountryId1 { get; set; }
    public int? TenantId { get; set; }
    public bool DocumentRequired { get; set; }
}