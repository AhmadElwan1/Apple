using Domain.Abstractions;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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

<<<<<<< HEAD
<<<<<<< HEAD
=======
=======
>>>>>>> b0815ad7cc9299dfe29de151779d08f342a0c371
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "HR Management System API",
                    Version = "v1"
                });
            });
<<<<<<< HEAD
>>>>>>> b0815ad (Error)
=======
>>>>>>> b0815ad7cc9299dfe29de151779d08f342a0c371

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