using Domain.Abstractions;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Infrastructure.RulesEngineDemo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Models;
using System.Text.Json;

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
                string rulesFilePath = "C:\\Users\\Ahmad-Elwan\\source\\repos\\Apple\\Domain\\Rules\\Rules.json";
                if (!File.Exists(rulesFilePath))
                {
                    throw new FileNotFoundException("Rules JSON file not found.", rulesFilePath);
                }

                string rulesJson = File.ReadAllText(rulesFilePath);

                Workflow workflow = JsonSerializer.Deserialize<Workflow>(rulesJson);
                if (workflow == null || workflow.Rules == null || !workflow.Rules.Any())
                {
                    throw new InvalidOperationException("Deserialized workflow is null or empty.");
                }

                string workflowJson = JsonSerializer.Serialize(workflow);
                return new RulesEngine.RulesEngine(new[] { workflowJson }, null);
            });
        }
    }
}
