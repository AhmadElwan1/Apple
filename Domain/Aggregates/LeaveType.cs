using Domain.Entities;
using Domain.Entities.LeaveType;
using static Domain.Enums.LeaveTypeEnums;

namespace Domain.Aggregates;

public class LeaveType
{

    public int Id { get; set; }
    public string Name { get; set; }
    public Validity validity { get; set; }
    public bool IsPaid { get; set; }
    public bool DocsRequired { get; set; }
    public Status Status { get; set; }




    public int CountryId { get; set; }
    public Country country { get; set; }


    public int TenantId { get; set; }
    public Tenant tenant { get; set; }


    public EntitlementRule entitlementRule { get; set; }
    public int EntitlementId { get; set; }


    public BalanceRule BalanceRule { get; set; }
    public int BalanceRuleId { get; set; }


    public ConsumptionRule ConsumptionRule { get; set; }
    public int ConsumptionRuleId { get; set; }


    public LeaveWorkflow LeaveWorkflow { get; set; }
    public int LeaveWorkflowId { get; set; }

}