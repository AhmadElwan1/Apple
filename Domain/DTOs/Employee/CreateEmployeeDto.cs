namespace Domain.DTOs.Employee
{
    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public bool IsMarried { get; set; }
        public bool HasNewBorn { get; set; }
        public int UnitId { get; set; }
        public int CountryId { get; set; }
        public int Gender { get; set; }
    }
}