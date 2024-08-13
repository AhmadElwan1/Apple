using Domain.Entities;
using Infrastructure.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Domain.Rules;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Routes
{
    public static class TenantRoutes
    {
        public static void MapTenantRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("/tenants", async (LeaveDbContext dbContext, string name) =>
            {
                Tenant tenant = new Tenant { Name = name };
                dbContext.Tenants.Add(tenant);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/tenants/{tenant.Id}", tenant);
            }).WithTags("Tenants");

            endpoints.MapPost("/tenants/{tenantId}/leave-rules", async (LeaveDbContext dbContext, int tenantId, LeaveRule rule) =>
            {
                Tenant? tenant = await dbContext.Tenants.FindAsync(tenantId);
                if (tenant == null)
                {
                    return Results.NotFound("Tenant not found.");
                }

                Country? country = await dbContext.Countries
                    .Include(c => c.LeaveRules)
                    .FirstOrDefaultAsync(c => c.LeaveRules.Any(lr => lr.TenantId == tenantId));

                if (country == null || country.Status != "Active")
                {
                    return Results.NotFound("Active country with leave rules not found.");
                }

                LeaveRule? existingRule = await dbContext.LeaveRules
                    .FirstOrDefaultAsync(r => r.TenantId == tenantId && r.RuleName == rule.RuleName);

                if (existingRule != null)
                {
                    existingRule.Expression = rule.Expression;
                    existingRule.SuccessEvent = rule.SuccessEvent;
                    existingRule.FailureEvent = rule.FailureEvent;
                    existingRule.CountryId = rule.CountryId;
                }
                else
                {
                    rule.TenantId = tenantId;
                    dbContext.LeaveRules.Add(rule);
                }

                await dbContext.SaveChangesAsync();
                return Results.Ok(rule);
            }).WithTags("Tenants");


        }
    }
}
