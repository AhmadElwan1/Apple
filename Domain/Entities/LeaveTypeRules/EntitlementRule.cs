using static Domain.Enums.LeaveTypeEnums;
using static Domain.Enums.EmployeeEnums;
namespace Domain.Entities.LeaveType;

public class EntitlementRule
{

    public int Id { get; set; }
    public bool IsTriggered { get; set; }
    public Unit? unit { get; set; }
    public Gender Gender { get; set; }
    public EmploymentType EmploymentType { get; set; }
    public string? Title { get; set; }
    public string ReportingTo { get; set; }
    public MaritalStatus maritalStatus { get; set; }

}
