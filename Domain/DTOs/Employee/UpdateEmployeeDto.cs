using static Domain.Enums.EmployeeEnums;

namespace Domain.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public bool? HasNewBorn { get; set; }
        public float? YearsOfService { get; set; }
        public Gender? Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public Religion? Religion { get; set; }
    }

}