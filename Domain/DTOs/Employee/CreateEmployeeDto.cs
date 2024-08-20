using Domain.Enums;

namespace Domain.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public bool HasNewBorn { get; set; }
        public float YearsOfService { get; set; }
        public int UnitId { get; set; }
        public int CountryId { get; set; }
        public EmployeeEnums.Gender Gender { get; set; }
        public EmployeeEnums.MaritalStatus MaritalStatus { get; set; }
        public EmployeeEnums.Religion Religion { get; set; }
    }

}