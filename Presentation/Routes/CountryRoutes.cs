using Domain.Abstractions;
using Domain.DTOs.Country;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Domain.Aggregates;
using Domain.DTOs.LeaveType;

namespace Presentation.Routes
{
    public static class CountryRoutes
    {
        public static void MapCountryRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/countries", async (CreateCountryDto createCountryDto, IValidator<CreateCountryDto> validator, ICountryRepository countryRepository) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(createCountryDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }
                
                await countryRepository.CreateCountryAsync(createCountryDto.Name);
                return Results.Ok($"Created Country {createCountryDto.Name}");
            }).WithTags("Country");

            endpoints.MapPost("/countries/{countryId}/leave-types", async (int countryId, LeaveTypeDto leaveTypeDto, IValidator<LeaveTypeDto> validator, ICountryRepository countryRepository) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(leaveTypeDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                LeaveType leaveType = await countryRepository.AddLeaveTypeAsync(countryId, leaveTypeDto);
                return Results.Created($"/countries/{countryId}/leave-types/{leaveType.LeaveTypeId}", leaveType);
            }).WithTags("Country");

            endpoints.MapPatch("/countries/{id}/activate", async (int id, ICountryRepository countryRepository) =>
            {
                bool activated = await countryRepository.ActivateCountryAsync(id);
                if (!activated)
                {
                    return Results.NotFound("Country not found or no rules to activate.");
                }

                return Results.Ok();
            }).WithTags("Country");

            endpoints.MapGet("/countries", async (ICountryRepository countryRepository) =>
            {
                IEnumerable<Country> countries = await countryRepository.GetAllCountriesAsync();
                return Results.Ok(countries);
            }).WithTags("Country");

            endpoints.MapDelete("/countries/{id}", async (int id, ICountryRepository countryRepository) =>
            {
                bool deleted = await countryRepository.DeleteCountryAsync(id);
                if (!deleted)
                {
                    return Results.NotFound("Country not found.");
                }

                return Results.NoContent();
            }).WithTags("Country");

            endpoints.MapDelete("/countries/{countryId}/leave-types/{leaveTypeId}", async (int countryId, int leaveTypeId, ICountryRepository countryRepository) =>
            {
                bool deleted = await countryRepository.DeleteLeaveTypeAsync(leaveTypeId);
                if (deleted)
                {
                    return Results.NoContent();
                }
                return Results.NotFound("Leave type not found.");
            }).WithTags("Country");

            endpoints.MapGet("/leave-types", async (ILeaveTypeRepository leaveTypeRepository) =>
            {
                IEnumerable<LeaveType> leaveTypes = await leaveTypeRepository.GetAllLeaveTypesAsync();
                return Results.Ok(leaveTypes);
            }).WithTags("LeaveTypes");
        }
    }
}
