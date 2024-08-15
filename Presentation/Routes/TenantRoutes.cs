using Domain.Abstractions;
using Domain.DTOs.Tenant;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Routes
{
    public static class TenantRoutes
    {
        public static void MapTenantRoutes(this IEndpointRouteBuilder endpoints)
        {
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
        }
    }
}