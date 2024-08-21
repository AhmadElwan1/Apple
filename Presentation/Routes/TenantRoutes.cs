using Domain.Abstractions;
using Domain.Aggregates;
using Domain.DTOs.Tenant;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.DTOs.LeaveType;

namespace Presentation.Routes
{
    public static class TenantRoutes
    {
        public static void MapTenantRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/tenants", async (ITenantRepository tenantRepository, [FromBody] CreateTenantDto createTenantDto, IValidator<CreateTenantDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(createTenantDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                Tenant tenant = await tenantRepository.CreateTenantAsync(createTenantDto.Name);
                return Results.Created($"/tenants/{tenant.Id}", tenant);
            }).WithTags("Tenants");

            endpoints.MapPut("/tenants/{tenantId}", async (ITenantRepository tenantRepository, int tenantId, [FromBody] UpdateTenantDto updateTenantDto, IValidator<UpdateTenantDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(updateTenantDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                bool updated = await tenantRepository.UpdateTenantNameAsync(tenantId, updateTenantDto.Name);
                if (updated)
                {
                    return Results.Ok();
                }
                return Results.NotFound("Tenant not found.");
            }).WithTags("Tenants");

            endpoints.MapPatch("/tenants/{tenantId}", async (ITenantRepository tenantRepository, int tenantId, [FromBody] UpdateTenantDto updateTenantDto, IValidator<UpdateTenantDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(updateTenantDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                bool updated = await tenantRepository.UpdateTenantNameAsync(tenantId, updateTenantDto.Name);
                if (updated)
                {
                    return Results.Ok();
                }
                return Results.NotFound("Tenant not found.");
            }).WithTags("Tenants");

            endpoints.MapPost("/tenants/{tenantId}/leave-types", async (int tenantId, [FromBody] LeaveTypeDto leaveTypeDto, ILeaveTypeRepository leaveTypeRepository, IValidator<LeaveTypeDto> validator) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(leaveTypeDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                LeaveType leaveType = await leaveTypeRepository.AddLeaveTypeAsync(tenantId, leaveTypeDto);

                return Results.Created($"/tenants/{tenantId}/leave-types/{leaveType.LeaveTypeId}", leaveType);
            }).WithTags("Tenants");

            endpoints.MapDelete("/rules/{ruleId}", async (ITenantRepository tenantRepository, int ruleId) =>
            {
                bool deleted = await tenantRepository.DeleteLeaveRuleAsync(ruleId);
                if (deleted)
                {
                    return Results.NoContent();
                }
                return Results.NotFound("Leave rule not found.");
            }).WithTags("Tenants");

            endpoints.MapDelete("/tenants/{tenantId}", async (ITenantRepository tenantRepository, int tenantId) =>
            {
                bool deleted = await tenantRepository.DeleteTenantAsync(tenantId);
                if (deleted)
                {
                    return Results.NoContent();
                }
                return Results.NotFound("Tenant not found.");
            }).WithTags("Tenants");

            endpoints.MapGet("/tenants", async (ITenantRepository tenantRepository) =>
            {
                IEnumerable<Tenant> tenants = await tenantRepository.GetAllTenantsAsync();
                return Results.Ok(tenants);
            }).WithTags("Tenants");
        }
    }
}
