using Domain.Abstractions;
using Domain.DTOs.LeaveRequest;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Routes
{
    public static class LeaveRequestRoutes
    {
        public static void MapLeaveRequestRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/leave-requests", async (ILeaveRequestRepository leaveRequestRepository) =>
            {
                IEnumerable<LeaveRequest> leaveRequests = await leaveRequestRepository.GetAllLeaveRequestsAsync();
                return Results.Ok(leaveRequests);
            }).WithTags("LeaveRequests");

            endpoints.MapGet("/leave-requests/{id}", async (int id, ILeaveRequestRepository leaveRequestRepository) =>
            {
                LeaveRequest? leaveRequest = await leaveRequestRepository.GetLeaveRequestByIdAsync(id);
                if (leaveRequest == null)
                {
                    return Results.NotFound("Leave request not found.");
                }

                return Results.Ok(leaveRequest);
            }).WithTags("LeaveRequests");

            endpoints.MapPost("/leave-requests", async (LeaveRequestDto leaveRequestDto, ILeaveRequestService leaveRequestService) =>
            {
                if (string.IsNullOrEmpty(leaveRequestDto.LeaveTypeName))
                {
                    return Results.BadRequest("LeaveTypeName is required.");
                }

                Employee? employee = await leaveRequestService.GetEmployeeByIdAsync(leaveRequestDto.EmployeeId);
                if (employee == null)
                {
                    return Results.NotFound("Employee not found.");
                }

                string resultMessage = await leaveRequestService.ApproveLeaveRequestAsync(leaveRequestDto.EmployeeId, leaveRequestDto.LeaveTypeName);

                if (resultMessage.Contains("not found") || resultMessage.Contains("not approved"))
                {
                    return Results.NotFound(resultMessage);
                }

                return Results.Created($"/leave-requests/{leaveRequestDto.EmployeeId}", new { message = resultMessage });
            }).WithTags("LeaveRequests");



            endpoints.MapPut("/leave-requests/{id}", async (int id, LeaveRequest leaveRequest, ILeaveRequestRepository leaveRequestRepository) =>
            {
                if (leaveRequest == null || leaveRequest.Id != id)
                {
                    return Results.BadRequest("Invalid leave request data.");
                }

                LeaveRequest? existingRequest = await leaveRequestRepository.GetLeaveRequestByIdAsync(id);
                if (existingRequest == null)
                {
                    return Results.NotFound("Leave request not found.");
                }

                await leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
                await leaveRequestRepository.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("LeaveRequests");

            endpoints.MapDelete("/leave-requests/{id}", async (int id, ILeaveRequestRepository leaveRequestRepository) =>
            {
                LeaveRequest? existingRequest = await leaveRequestRepository.GetLeaveRequestByIdAsync(id);
                if (existingRequest == null)
                {
                    return Results.NotFound("Leave request not found.");
                }

                await leaveRequestRepository.DeleteLeaveRequestAsync(id);
                await leaveRequestRepository.SaveChangesAsync();
                return Results.NoContent();
            }).WithTags("LeaveRequests");

            endpoints.MapGet("/leave-requests/employee/{employeeId}", async (int employeeId, ILeaveRequestRepository leaveRequestRepository) =>
           {
               IEnumerable<LeaveRequest> leaveRequests = await leaveRequestRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);
               if (!leaveRequests.Any())
               {
                   return Results.NotFound("No leave requests found for this employee.");
               }

               return Results.Ok(leaveRequests);
           }).WithTags("LeaveRequests");
        }
    }
}