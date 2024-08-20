using static Domain.Enums.EmployeeEnums;

namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool HasNewBorn { get; set; }
    public float YearsOfService { get; set; }
    public int UnitId { get; set; }
    public int CountryId { get; set; }
    public Gender Gender { get; set; } 
    public MaritalStatus MaritalStatus { get; set; }
    public Religion Religion { get; set; }
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}