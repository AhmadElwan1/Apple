using Domain.DTOs.LeaveType;
using Domain.Entities;

namespace Domain.Aggregates
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }

        public string LeaveTypeName { get; set; }
        public string Entilement { get; set; }
        public string Accural { get; set; }
        public string CarryOver { get; set; }
        public string Expression { get; set; }
        public string NoticePeriod { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public bool DocumentRequired { get; set; }
    }
}