using Domain.Entities.LeaveType.Balance;
using static Domain.Enums.LeaveTypeEnums;

namespace Domain.Entities.LeaveType;

public class BalanceRule
{

    public int Id { get; set; }
    public int MinLegalBalance { get; set; }
    public BalanceCalculationRule balanceCalculationRule { get; set; }
    public int ExtendableBalanceMax { get; set; }
    public bool isOverridable { get; set; }
    public int GracePeriod { get; set; }
    public Frequency? frequency { get; set; }
    public DateTime? FrequencyStartFrom { get; set; }


}
