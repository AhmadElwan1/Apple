namespace Domain.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public bool? IsMarried { get; set; }
        public bool? HasNewBorn { get; set; }
    }
}