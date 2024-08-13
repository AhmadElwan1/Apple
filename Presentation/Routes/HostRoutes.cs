using Domain.Entities;
using Infrastructure.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Routes;

public static class HostRoutes
{
    public static void MapHostRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/countries", async (LeaveDbContext dbContext, string name) =>
        {
            Country country = new Country { Name = name };
            dbContext.Countries.Add(country);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/countries/{country.Id}", country);
        }).WithTags("Host");

        endpoints.MapPatch("/countries/{id}/activate", async (LeaveDbContext dbContext, int id) =>
        {
            Country? country = await dbContext.Countries
                .Include(c => c.LeaveRules)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (country == null)
                return Results.NotFound("Country not found.");

            if (!country.LeaveRules.Any())
                return Results.BadRequest("Country must have associated rules before activation.");

            country.Status = "Active";
            await dbContext.SaveChangesAsync();

            return Results.Ok(country);
        }).WithTags("Host");
    }
}