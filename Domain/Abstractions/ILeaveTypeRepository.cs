using Domain.DTOs.LeaveType;

namespace Domain.Abstractions
{
    public interface ILeaveTypeRepository
    {
        Task<LeaveType> AddLeaveTypeAsync(int countryId, LeaveTypeDto leaveTypeDto);

        Task<LeaveType?> GetLeaveTypeByIdAsync(int leaveTypeId);

        Task<LeaveType?> UpdateLeaveTypeAsync(int leaveTypeId, LeaveTypeDto leaveTypeDto);

        Task<bool> DeleteLeaveTypeAsync(int leaveTypeId);

        Task<IEnumerable<LeaveType>> GetAllLeaveTypesAsync();
    }
}