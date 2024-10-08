﻿using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FluentValidation;
using FluentValidation.Results;
using Domain.DTOs.Unit;

namespace Presentation.Routes
{
    public static class UnitRoutes
    {
        public static void MapUnitRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/units", async (IUnitRepository unitRepository, CreateUnitDto createUnitDto, IValidator<CreateUnitDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(createUnitDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                Unit unit = await unitRepository.CreateUnitAsync(createUnitDto);
                return Results.Created($"/units/{unit.Id}", unit);
            }).WithTags("Units");

            endpoints.MapPut("/units/{unitId}", async (IUnitRepository unitRepository, int unitId, UpdateUnitDto updateUnitDto, IValidator<UpdateUnitDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(updateUnitDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                bool updated = await unitRepository.UpdateUnitAsync(unitId, updateUnitDto);
                if (updated)
                {
                    return Results.Ok();
                }

                return Results.NotFound("Unit not found.");
            }).WithTags("Units");

            endpoints.MapPatch("/units/{unitId}", async (IUnitRepository unitRepository, int unitId, UpdateUnitDto updateUnitDto, IValidator<UpdateUnitDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(updateUnitDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                bool updated = await unitRepository.UpdateUnitAsync(unitId, updateUnitDto);
                if (updated)
                {
                    return Results.Ok();
                }

                return Results.NotFound("Unit not found.");
            }).WithTags("Units");

            endpoints.MapDelete("/units/{unitId}", async (IUnitRepository unitRepository, int unitId) =>
            {
                bool deleted = await unitRepository.DeleteUnitAsync(unitId);
                if (deleted)
                {
                    return Results.NoContent();
                }

                return Results.NotFound("Unit not found.");
            }).WithTags("Units");

            endpoints.MapGet("/units", async (IUnitRepository unitRepository) =>
            {
                IEnumerable<Unit> units = await unitRepository.GetAllUnitsAsync();
                return Results.Ok(units);
            }).WithTags("Units");

            endpoints.MapGet("/units/tenant/{tenantId}", async (IUnitRepository unitRepository, int tenantId) =>
            {
                IEnumerable<Unit> units = await unitRepository.GetAllUnitsByTenantIdAsync(tenantId);
                return Results.Ok(units);
            }).WithTags("Units");

            endpoints.MapGet("/units/{unitId}", async (IUnitRepository unitRepository, int unitId) =>
            {
                Unit? unit = await unitRepository.GetUnitByIdAsync(unitId);
                if (unit != null)
                {
                    return Results.Ok(unit);
                }

                return Results.NotFound("Unit not found.");
            }).WithTags("Units");
        }
    }
}
