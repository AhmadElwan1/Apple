## **Overview**
Apple is a demonstration project for implementing a rules engine using C#. This project showcases how to build a flexible and maintainable rules engine that can be used to evaluate complex business rules.

## **Repository Structure**
The repository is organized into the following main folders:

- **Apple.Api**: Contains the API layer of the application.
- **Apple.Presentation**: Contains the routes of the application.
- **Apple.Domain**: Defines the domain models and entities.
- **Apple.Infrastructure**: Implements services and data access.

## **Getting Started**
### **Prerequisites**
- .NET SDK 
- IDE (e.g., Visual Studio, Visual Studio Code)

### **Installation**
1. Clone the repository:
    ```sh
    git clone https://github.com/AhmadElwan1/Apple.git
    cd Apple
    ```
2. Restore dependencies and build the solution:
    ```sh
    dotnet restore
    dotnet build
    ```
3. Running The Application:
    ```sh
    cd RulesEngineDemo.Api
    dotnet run
    ```

## **Domain Layer**

### **Entities**
- **Country**

    Represents a country within the system; because each country has its own rules by law.
    ```csharp
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } = "Draft";
        
        public ICollection<LeaveType> LeaveTypes { get; set; } = new List<LeaveType>();
    }
    ```

- **Tenant**

    Represents a tenant within the system; the client that will buy the system.
    ```csharp
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<LeaveType> LeaveTypes { get; set; } = new List<LeaveType>();
    }
    ```

- **Unit**

    Represents a Unit inside the tenant; a part of the organization chart.
    ```csharp
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TenantId { get; set; }
    }
    ```

- **Employee**

    Represents an Employee within the system; the person that works as a part of a unit in the tenant.
    ```csharp
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasNewBorn { get; set; }
        public float YearsOfService { get; set; }
        public int UnitId { get; set; }
        public int CountryId { get; set; }
        public Gender Gender { get; set; } 
        public MaritalStatus MaritalStatus { get; set; }
        public Religion Religion { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
    ```

- **LeaveRequest**

    Represents a Leave Rule within the system (to be decided if we need to separate by country, tenant, or both).
    ```csharp
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public string LeaveTypeName { get; set; }
    }
    ```

### **Rules**

- **Rules**

    Represents a JSON file containing the rules that will be checked for each leave request.
    ```json
        {
    "WorkflowName": "LeaveRequestWorkflow",
    "Rules": [
        {
        "RuleName": "FatherhoodLeaveRule",
        "Expression": "employee.HasNewBorn && employee.YearsOfService > 0 && employee.Gender == 0",
        "SuccessEvent": "Fatherhood Leave Approved for 3 days",
        "FailureEvent": "Fatherhood Leave Not Approved"
        },
        {
        "RuleName": "AnnualLeaveRuleLessThan5",
        "Expression": "employee.YearsOfService > 0 && employee.YearsOfService < 5",
        "SuccessEvent": "Annual Leave Approved for 14 Days",
        "FailureEvent": "Annual Leave Not Approved"
        },
        {
        "RuleName": "AnnualLeaveRuleMoreThan5",
        "Expression": "employee.YearsOfService >= 5",
        "SuccessEvent": "Annual Leave Approved for 21 Days",
        "FailureEvent": "Annual Leave Not Approved"
        },
        {
        "RuleName": "SickLeaveRule",
        "Expression": "employee.YearsOfService > 0",
        "SuccessEvent": "Sick Leave Approved for 1 Day",
        "FailureEvent": "Sick Leave Not Approved"
        },
        {
        "RuleName": "MaternityLeaveRule",
        "Expression": "employee.Gender == 1 && employee.HasNewBorn",
        "SuccessEvent": "Maternity Leave Approved for 2 Months",
        "FailureEvent": "Maternity Leave Not Approved"
        }
    ]
    }
    ```

** ###Rules### **

**Rules**
Represents the rules for input validation.
```csharp
public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(1, 25).WithMessage("Name must be between 1 and 25 characters.");

            RuleFor(x => x.YearsOfService)
                .GreaterThanOrEqualTo(0).WithMessage("YearsOfService must be a non-negative number.");

            RuleFor(x => x.UnitId)
                .GreaterThan(0).WithMessage("UnitId must be a positive integer.");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).WithMessage("CountryId must be a positive integer.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Gender is not valid.");

            RuleFor(x => x.MaritalStatus)
                .IsInEnum().WithMessage("MaritalStatus is not valid.");

            RuleFor(x => x.Religion)
                .IsInEnum().WithMessage("Religion is not valid.");

            RuleFor(x => x.HasNewBorn)
                .NotNull().WithMessage("HasNewBorn must be specified.");
        }
    }
```
*You can check the rest of the validators inside the validators folder.*

### **DTOs**
Represents the DTO for each action so the entity in the database won't be affected directly from the API and to form the input for each action as needed.
```csharp
public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public bool? HasNewBorn { get; set; }
        public float? YearsOfService { get; set; }
        public Gender? Gender { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public Religion? Religion { get; set; }
    }
```
*You can check the rest of the DTOs inside the DTOs folder.*

### **Abstractions**
Represents the interfaces.
```csharp
public interface ICountryRepository
    {
        Task<Country> CreateCountryAsync(string name);
        Task<bool> ActivateCountryAsync(int id);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<LeaveType> AddLeaveTypeAsync(int countryId, LeaveTypeDto leaveTypeDto);
        Task<bool> DeleteCountryAsync(int id);
        Task<bool> DeleteLeaveTypeAsync(int ruleId);
    }
```
*You can check the rest of the abstractions inside the abstractions folder.*

### **Aggregates**
Represents the aggregates.
```csharp
public class LeaveType
    {
        public int LeaveTypeId { get; set; }

        public string LeaveTypeName { get; set; }
        public string Entilement { get; set; }
        public string Accural { get; set; }
        public string CarryOver { get; set; }
        public string Expression { get; set; }
        public string NoticePeriod { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public bool DocumentRequired { get; set; }
    }
```

### **Enums**
Represents the Enums.
```csharp
public class EmployeeEnums
    {
        public enum Gender
        {
            Male = 0,
            Female = 1
        }

        public enum MaritalStatus
        {
            Single = 0,
            Engaged = 1,
            Married = 2,
            Divorced = 3
        }

        public enum Religion
        {
            Christian = 0,
            Muslim = 1,
            Other = 2,
        }
    }
```

## **Infrastructure Layer**

### **LeaveDbContext**
```csharp
public class LeaveDbContext : DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options)
            : base(options)
        {
        }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Employee> Employees { get; set;}
        public DbSet<Country> Countries { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new TenantConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveRequestConfiguration());
        }
    }
```
*Represents the Entity Framework Core database context for managing leave-related entities such as tenants, units, employees, countries, leave types, and leave requests within the application. It serves as the primary class for interacting with the database.*

### **Adding The Services Needed**
```csharp
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
```

### **Repositories**
*Inside the repositories folder you will find the implementation of the interfaces defined in the domain layer.*
```csharp
public class UnitRepository : IUnitRepository
    {
        private readonly LeaveDbContext _dbContext;

        public UnitRepository(LeaveDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit?> GetUnitByIdAsync(int unitId)
        {
            Unit? unit = await _dbContext.Units
                .FirstOrDefaultAsync(u => u.Id == unitId);

            return unit;
        }

        public async Task<Unit> CreateUnitAsync(CreateUnitDto createUnitDto)
        {
            Unit unit = new Unit
            {
                Name = createUnitDto.Name,
                TenantId = createUnitDto.TenantId
            };

            _dbContext.Units.Add(unit);
            await _dbContext.SaveChangesAsync();
            return unit;
        }

        public async Task<bool> UpdateUnitAsync(int unitId, UpdateUnitDto updateUnitDto)
        {
            Unit? unit = await _dbContext.Units.FindAsync(unitId);
            if (unit == null) return false;

            if (updateUnitDto.Name != null)
                unit.Name = updateUnitDto.Name;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUnitAsync(int unitId)
        {
            Unit? unit = await _dbContext.Units.FindAsync(unitId);
            if (unit == null) return false;

            _dbContext.Units.Remove(unit);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsByTenantIdAsync(int tenantId)
        {
            return await _dbContext.Units
                .Where(u => u.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Unit>> GetAllUnitsAsync()
        {
            return await _dbContext.Units.ToListAsync();
        }
    }
```
*You can check the rest of the services implemented inside the repositories folder.*

### **Configurations**
*Here you will find the configuration for each entity.*

- **UnitConfiguration**: The relationship configuration for the Unit entity.
```csharp
public class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

## **RulesEngineDemo**
*This section describes the configuration and implementation details for the LeaveRequestService and its related components.*

- **LeaveRequestService**: Handles the logic for processing leave requests within the system. 

```csharp
public class LeaveRequestService : ILeaveRequestService
{
    private readonly LeaveDbContext _context;
    private readonly RulesEngine.RulesEngine _rulesEngine;

    public LeaveRequestService(LeaveDbContext dbContext, RulesEngine.RulesEngine rulesEngine)
    {
        _context = dbContext;
        _rulesEngine = rulesEngine;
    }

    public async Task<string> ApproveLeaveRequestAsync(int employeeId, string leaveTypeName)
    {
        // Fetch employee
        Employee? employee = await _context.Employees.FindAsync(employeeId);
        if (employee == null)
        {
            return "Employee not found.";
        }

        // Check for existing approved leave
        bool hasApprovedLeave = await _context.LeaveRequests
            .AnyAsync(lr => lr.EmployeeId == employeeId && lr.IsApproved);

        if (hasApprovedLeave)
        {
            return "An approved leave already exists for this employee.";
        }

        // Execute rules
        List<RuleResultTree> results = await ExecuteRulesAsync(employee);

        // Determine if the leave is approved based on rule results
        RuleResultTree? successfulRule = results.FirstOrDefault(r => r.Rule.RuleName == leaveTypeName && r.IsSuccess);
        string approvalMessage = successfulRule != null ? successfulRule.Rule.SuccessEvent : "Leave Not Approved";

        // Get leave duration and message
        (TimeSpan duration, string message) = GetLeaveDurationAndMessage(approvalMessage);

        // Create leave request regardless of approval status
        DateTime startDate = DateTime.UtcNow;
        DateTime endDate = startDate.Add(duration);
        await CreateLeaveRequestAsync(employeeId, startDate, endDate, successfulRule != null && successfulRule.IsSuccess, leaveTypeName);

        return message;
    }

    private async Task<List<RuleResultTree>> ExecuteRulesAsync(Employee employee)
    {
        RuleParameter employeeRuleParams = new RuleParameter("employee", employee);
        return await _rulesEngine.ExecuteAllRulesAsync("LeaveRequestWorkflow", employeeRuleParams);
    }

    private (TimeSpan, string) GetLeaveDurationAndMessage(string successEvent)
    {
        Dictionary<string, (TimeSpan, string)> leaveDurations = new Dictionary<string, (TimeSpan, string)>
        {
            { "Fatherhood Leave Approved for 3 days", (TimeSpan.FromDays(3), "Leave Approved for 3 Days") },
            { "Annual Leave Approved for 14 Days", (TimeSpan.FromDays(14), "Leave Approved for 14 Days") },
            { "Annual Leave Approved for 21 Days", (TimeSpan.FromDays(21), "Leave Approved for 21 Days") },
            { "Sick Leave Approved for 1 Day", (TimeSpan.FromDays(1), "Leave Approved for 1 Day") },
            { "Maternity Leave Approved for 2 Months", (TimeSpan.FromDays(60), "Leave Approved for 2 Months") }
        };

        return leaveDurations.TryGetValue(successEvent, out (TimeSpan, string) result) ? result : (TimeSpan.Zero, "Leave Not Approved");
    }

    private async Task CreateLeaveRequestAsync(int employeeId, DateTime startDate, DateTime endDate, bool isApproved, string leaveTypeName)
    {
        LeaveRequest leaveRequest = new LeaveRequest
        {
            EmployeeId = employeeId,
            StartDate = startDate,
            EndDate = endDate,
            IsApproved = isApproved,
            LeaveTypeName = leaveTypeName
        };

        await _context.LeaveRequests.AddAsync(leaveRequest);
        await _context.SaveChangesAsync();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        return await _context.Employees.FindAsync(employeeId);
    }
}
```

## **Presentation Layer**

### **Routes**
*This folder contains the endpoints implementation.*
```csharp
endpoints.MapPost("/units", async (IUnitRepository unitRepository, CreateUnitDto createUnitDto, IValidator<CreateUnitDto> validator) =>
            {
                ValidationResult? validationResult = await validator.ValidateAsync(createUnitDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                Unit unit = await unitRepository.CreateUnitAsync(createUnitDto);
                return Results.Created($"/units/{unit.Id}", unit);
            }).WithTags("Units");
```
*You can check the rest of the routes implemented inside the Routes folder.*

## **Presentation.Api**
This is a .NET 8 web API project.

### **appsettings.json**
Here we store configuration settings for the application, including the database connection string and other key-value pairs for application behavior, environment-specific settings, and external service configurations.

### **ApplicationServiceExtensions**

*This class is responsible for setting up application services, middleware, and routes. It ensures that all necessary components are properly configured for the application.*

```csharp
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
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateCountryDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateTenantDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateTenantDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateUnitDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateUnitDtoValidator)));
                fv.RegisterValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateEmployeeDtoValidator)));
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
```


### **Program.cs File**

*here we register the services and dependencies that we need in order for our application to run correctly*

```csharp
using Presentation.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

WebApplication app = builder.Build();

app.AddRoutes();
app.AddUsings();
app.Run();
```