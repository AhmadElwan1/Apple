using System.Reflection;
using Domain.DTOs.Country;
using Domain.DTOs.Tenant;
using Domain.Validators.CountryValidators;
using Domain.Validators.EmployeeValidators;
using Domain.Validators.TenantValidators;
using Domain.Validators.UnitValidators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Dependencies;
using Microsoft.OpenApi.Models;
using Presentation.Routes;


namespace Presentation.Api.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructureServices(config);

        services.AddScoped<IValidator<CreateCountryDto>, CreateCountryDtoValidator>();
        services.AddScoped<IValidator<CreateTenantDto>, CreateTenantDtoValidator>();
        services.AddScoped<IValidator<UpdateTenantDto>, UpdateTenantDtoValidator>();
        services.AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateEmployeeDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateTenantDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateTenantDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateUnitDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateUnitDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateCountryDtoValidator)));

            });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HR Management System API",
                Version = "v1"
            });
        });

        return services;
    }

    public static IApplicationBuilder AddUsings(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Management System API v1");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }

    public static IEndpointRouteBuilder AddRoutes(this IEndpointRouteBuilder route)
    {
        route.MapCountryRoutes();
        route.MapTenantRoutes();
        route.MapEmployeeRoutes();
        route.MapUnitRoutes();
        route.MapLeaveRequestRoutes();

        return route;
    }
}
