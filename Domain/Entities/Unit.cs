namespace Domain.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }

        public ICollection<int> EmployeeIds { get; set; } = new List<int>();
    }
}