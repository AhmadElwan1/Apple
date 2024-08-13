using Domain.DTOs;

namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsMarried { get; set; }
    public bool HasNewBorn { get; set; }
    public float YearsOfService { get; set; }
    public int Gender { get; set; }

    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
}