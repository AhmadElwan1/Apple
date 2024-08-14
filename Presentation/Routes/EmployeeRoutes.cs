using Domain.Abstractions;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Routes
{
    public static class EmployeeRoutes
    {
        public static void MapEmployeeRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/employees", async (HttpContext httpContext) =>
            {
                IEmployeeRepository employeeRepository = httpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                Employee? employee = await httpContext.Request.ReadFromJsonAsync<Employee>();

                if (employee == null)
                {
                    return Results.BadRequest("Employee data is required.");
                }

                employee = await employeeRepository.CreateEmployeeAsync(employee);
                return Results.Created($"/employees/{employee.Id}", employee);
            }).WithTags("Employees");

            endpoints.MapPost("/employees/{employeeId}/leave-requests", async (HttpContext httpContext, int employeeId) =>
            {
                IEmployeeRepository employeeRepository = httpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                LeaveRequest? leaveRequest = await httpContext.Request.ReadFromJsonAsync<LeaveRequest>();

                if (leaveRequest == null)
                {
                    return Results.BadRequest("Leave request data is required.");
                }

                LeaveRequest? createdLeaveRequest = await employeeRepository.CreateLeaveRequestAsync(employeeId, leaveRequest);
                if (createdLeaveRequest == null)
                {
                    return Results.NotFound("Employee not found.");
                }

                return Results.Created($"/leave-requests/{createdLeaveRequest.Id}", createdLeaveRequest);
            }).WithTags("Employees");

            endpoints.MapDelete("/employees/{employeeId}", async (HttpContext httpContext, int employeeId) =>
            {
                IEmployeeRepository employeeRepository = httpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                bool result = await employeeRepository.DeleteEmployeeAsync(employeeId);

                if (result)
                {
                    return Results.NoContent();
                }

                return Results.NotFound("Employee not found.");
            }).WithTags("Employees");

            endpoints.MapPut("/employees/{employeeId}", async (HttpContext httpContext, int employeeId) =>
            {
                IEmployeeRepository employeeRepository = httpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                Employee? updatedEmployee = await httpContext.Request.ReadFromJsonAsync<Employee>();

                if (updatedEmployee == null)
                {
                    return Results.BadRequest("Employee data is required.");
                }

                bool result = await employeeRepository.UpdateEmployeeAsync(employeeId, updatedEmployee);
                if (result)
                {
                    return Results.Ok();
                }

                return Results.NotFound("Employee not found.");
            }).WithTags("Employees");

            endpoints.MapPatch("/employees/{employeeId}", async (HttpContext httpContext, int employeeId) =>
            {
                IEmployeeRepository employeeRepository = httpContext.RequestServices.GetRequiredService<IEmployeeRepository>();
                UpdateEmployeeDto? updateEmployeeDto = await httpContext.Request.ReadFromJsonAsync<UpdateEmployeeDto>();

                if (updateEmployeeDto == null)
                {
                    return Results.BadRequest("Update data is required.");
                }

                bool result = await employeeRepository.PartialUpdateEmployeeAsync(employeeId, updateEmployeeDto);
                if (result)
                {
                    return Results.Ok();
                }

                return Results.NotFound("Employee not found.");
            }).WithTags("Employees");
        }
    }
}
