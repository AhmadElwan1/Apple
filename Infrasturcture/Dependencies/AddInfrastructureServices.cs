using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RulesEngine.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dependencies
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LeaveDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ILeaveRulesRepository, LeaveRulesRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();

            services.AddSingleton<RulesEngine.RulesEngine>(sp =>
            {
                IConfiguration config = sp.GetRequiredService<IConfiguration>();
                string rulesFilePath = config.GetValue<string>("C:\\Users\\Ahmad-Elwan\\source\\repos\\Apple\\Domain\\Rules\\Rules.json")!;
                string rulesJson = File.ReadAllText(rulesFilePath);
                Workflow workflow = JsonConvert.DeserializeObject<Workflow>(rulesJson)!;
                return new RulesEngine.RulesEngine(new[] { workflow }, null);
            });
        }
    }
}