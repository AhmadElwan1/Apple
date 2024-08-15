using Infrastructure.Dependencies;
using Presentation.Routes;
using FluentValidation.AspNetCore;
using System.Reflection;
using Domain.Validators.EmployeeValidators;
using Domain.Validators.LeaveRuleValidators;
using Domain.Validators.TenantValidators;
using Domain.Validators.UnitValidators;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateEmployeeDtoValidator)));
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateTenantDtoValidator)));
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateTenantDtoValidator)));
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateUnitDtoValidator)));
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateUnitDtoValidator)));
        fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(LeaveRuleDtoValidator)));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HR Management System API",
        Version = "v1"
    });
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
app.MapUnitRoutes();

app.Run();