using Domain.Entities;
using Infrastructure.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Domain.DTOs;

namespace Presentation.Routes
{
    public static class EmployeeRoutes
    {
        public static void MapEmployeeRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/employees", async (LeaveDbContext dbContext, Employee employee) =>
            {

                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/employees/{employee.Id}", employee);
            }).WithTags("Employees");

            endpoints.MapPost("/employees/{employeeId}/leave-requests", async (LeaveDbContext dbContext, int employeeId, LeaveRequest leaveRequest) =>
            {
                Employee? employee = await dbContext.Employees.FindAsync(employeeId);
                if (employee == null)
                {
                    return Results.NotFound("Employee not found.");
                }

                if (leaveRequest == null)
                {
                    return Results.BadRequest("Leave request data is required.");
                }

                leaveRequest.EmployeeId = employeeId;
                dbContext.LeaveRequests.Add(leaveRequest);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/leave-requests/{leaveRequest.Id}", leaveRequest);
            }).WithTags("Employees");
        }
    }
}