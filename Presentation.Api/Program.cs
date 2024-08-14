using Application;
using Domain.Abstractions;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RulesEngine.Models;
using Presentation.Routes;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LeaveDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ILeaveRulesRepository, LeaveRulesRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HR Management System API",
        Version = "v1"
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

builder.Services.AddSingleton<RulesEngine.RulesEngine>(sp =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    string rulesFilePath = configuration.GetValue<string>("C:\\Users\\Ahmad-Elwan\\source\\repos\\Apple\\Domain\\Rules\\Rules.json");
    string rulesJson = File.ReadAllText(rulesFilePath);
    Workflow workflow = JsonConvert.DeserializeObject<Workflow>(rulesJson);
    return new RulesEngine.RulesEngine(new[] { workflow }, null);
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Management System API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapCountryRoutes();
app.MapTenantRoutes();
app.MapEmployeeRoutes();

app.Run();