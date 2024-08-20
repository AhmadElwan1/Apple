using System.Text.Json;
using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Models;
using Microsoft.Extensions.Configuration;
using Infrastructure.DB;
using Infrastructure.RulesEngineDemo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dependencies
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LeaveDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();

            services.AddScoped<ILeaveRequestService, LeaveRequestService>();

            services.AddSingleton<RulesEngine.RulesEngine>(sp =>
            {
                string rulesFilePath = configuration.GetValue<string>("RulesFilePath")!;
                if (string.IsNullOrWhiteSpace(rulesFilePath))
                    throw new InvalidOperationException("Rules file path is not configured.");

                string rulesJson = File.ReadAllText(rulesFilePath);
                Workflow workflow = JsonSerializer.Deserialize<Workflow>(rulesJson)!;
                if (workflow.Rules == null || !workflow.Rules.Any())
                    throw new InvalidOperationException("Deserialized workflow is null or empty.");

                return new RulesEngine.RulesEngine(new[] { workflow }, null);
            });
        }
    }
}
