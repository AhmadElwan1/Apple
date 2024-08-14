namespace Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = "Draft";

        public ICollection<LeaveRule> LeaveRules { get; set; } = new List<LeaveRule>();
    }
}