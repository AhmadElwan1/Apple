using Domain.Abstractions;
using Domain.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Routes
{
    public static class CountryRoutes
    {
        public static void MapCountryRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/countries", async (ICountryRepository countryRepository, [FromBody] CreateCountryDto createCountryDto, IValidator<CreateCountryDto> validator) =>
            {
                ValidationResult result = await validator.ValidateAsync(createCountryDto);
                if (!result.IsValid)
                {
                    return Results.BadRequest(result.Errors);
                }

                Country country = await countryRepository.CreateCountryAsync(createCountryDto.Name);
                return Results.Created($"/countries/{country.Id}", country);
            }).WithTags("Country");

            endpoints.MapPost("/countries/{countryId}/rules", async (ICountryRepository countryRepository, int countryId, [FromBody] LeaveRuleDto leaveRuleDto, IValidator<LeaveRuleDto> validator) =>
            {
                ValidationResult result = await validator.ValidateAsync(leaveRuleDto);
                if (!result.IsValid)
                {
                    return Results.BadRequest(result.Errors);
                }

                LeaveRule? rule = await countryRepository.AddLeaveRuleAsync(countryId, leaveRuleDto);
                return Results.Created($"/countries/{countryId}/rules/{rule.Id}", rule);
            }).WithTags("Country");

            endpoints.MapPatch("/countries/{id}/activate", async (ICountryRepository countryRepository, int id) =>
            {
                bool activated = await countryRepository.ActivateCountryAsync(id);
                if (!activated)
                    return Results.NotFound("Country not found or no rules to activate.");

                return Results.Ok();
            }).WithTags("Country");

            endpoints.MapGet("/countries", async (ICountryRepository countryRepository) =>
            {
                IEnumerable<Country> countries = await countryRepository.GetAllCountriesAsync();
                return Results.Ok(countries);
            }).WithTags("Country");

            endpoints.MapDelete("/countries/{id}", async (ICountryRepository countryRepository, int id) =>
            {
                bool deleted = await countryRepository.DeleteCountryAsync(id);
                if (!deleted)
                    return Results.NotFound("Country not found.");

                return Results.NoContent();
            }).WithTags("Country");
        }
    }
}
