namespace Domain.Enums;

public class LeaveTypeEnums
{

    public enum Validity
    {
        Limited = 0,
        UnLimited = 1
    }

    public enum Status
    {
        Draft = 0,
        InActive = 1,
        Active = 2
    }

    public enum EmploymentType
    {
        FullTime = 0,
        PartTime = 1
    }

    public enum Entitled
    {
        ThroughBalance = 0,
        EventBased = 1
    }

    public enum Frequency
    {
        Yearly = 0,
        Monthly = 1,
        Once = 2
    }

    public enum ConsumptionType
    {
        Accural = 0,
        Accumulative = 1
    }

    public enum UnUsedBalance
    {
        WaveOut = 0,
        Cashement = 1,
        CarryOver = 2
    }
}
