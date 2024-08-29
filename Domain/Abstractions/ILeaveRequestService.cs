namespace Domain.Abstractions
{
    public interface ILeaveRequestService
    {
        Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName);
    }
}
