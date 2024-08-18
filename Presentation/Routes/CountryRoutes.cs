using Domain.Abstractions;
using Domain.DTOs.Country;
using Domain.DTOs.LeaveRule;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

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

                Country country = new Country { Name = createCountryDto.Name };
                await countryRepository.CreateCountryAsync(country.Name);
                return Results.Created($"/countries/{country.Id}", country);
            }).WithTags("Country");

            endpoints.MapPost("/countries/{countryId}/rules", async (int countryId, LeaveRuleDto leaveRuleDto, IValidator<LeaveRuleDto> validator, ICountryRepository countryRepository) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(leaveRuleDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                LeaveRule rule = await countryRepository.AddLeaveRuleAsync(countryId, leaveRuleDto);
                return Results.Created($"/countries/{countryId}/rules/{rule.Id}", rule);
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

            endpoints.MapDelete("/countries/{countryId}/rules/{ruleId}", async (int countryId, int ruleId, ICountryRepository countryRepository) =>
            {
                bool deleted = await countryRepository.DeleteLeaveRuleAsync(ruleId);
                if (deleted)
                {
                    return Results.NoContent();
                }
                return Results.NotFound("Leave rule not found.");
            }).WithTags("Country");

            endpoints.MapGet("/rules", async (ILeaveRulesRepository leaveRulesRepository) =>
            {
                IEnumerable<LeaveRule> leaveRules = leaveRulesRepository.GetAllRules();
                return Results.Ok(leaveRules);
            }).WithTags("LeaveRules");
        }
    }
}
