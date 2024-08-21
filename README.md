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
        
        public ICollection<LeaveRule> LeaveRules { get; set; } = new List<LeaveRule>();
    }
    ```

- **Tenant**

    Represents a tenant within the system; the client that will buy the system.
    ```csharp
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<LeaveRule> LeaveRules { get; set; } = new List<LeaveRule>();
        
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
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
        public Tenant Tenant { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
    ```

- **Employee**

    Represents an Employee within the system; the person that works as a part of a unit in the tenant.
    ```csharp
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsMarried { get; set; }
        public bool HasNewBorn { get; set; }
        public float YearsOfService { get; set; }
        public int Gender { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
    ```

- **LeaveRule**

    Represents a Leave Rule within the system (to be decided if we need to separate by country, tenant, or both).
    ```csharp
    public class LeaveRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Expression { get; set; }
        public string SuccessEvent { get; set; }
        public string FailureEvent { get; set; }
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
                "Expression": "employee.HasNewBorn && employee.YearsOfService >= 1 && employee.Gender == 0",
                "SuccessEvent": "Fatherhood Leave Approved for 3 days",
                "FailureEvent": "Fatherhood Leave Not Approved"
            }
        ]
    }
    ```

### **

s**
Represents the rules for input validation.
```csharp
public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 25).WithMessage("Name must be between 1 and 25 characters.");

        RuleFor(x => x.IsMarried)
            .NotNull().WithMessage("IsMarried must be specified.");

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
    public bool? IsMarried { get; set; }
    public bool? HasNewBorn { get; set; }
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
    Task<LeaveRule> AddLeaveRuleAsync(int countryId, LeaveRuleDto leaveRuleDto);
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<bool> DeleteCountryAsync(int id);
    Task<bool> DeleteLeaveRuleAsync(int ruleId);
}
```
*You can check the rest of the abstractions inside the abstractions folder.*

## **Infrastructure Layer**

### **LeaveDbContext**
```csharp
public class LeaveDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<LeaveRule> LeaveRules { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
```
*Represents the Entity Framework Core database context for managing leave-related entities such as tenants, units, employees, countries, leave rules, and leave requests within the application. It serves as the primary class for interacting with the database.*

### **Adding The Services Needed**
```csharp
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
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();


            services.AddSingleton<RulesEngine.RulesEngine>(sp =>
            {
                string rulesFilePath = configuration.GetValue<string>("RulesFilePath");
                if (string.IsNullOrWhiteSpace(rulesFilePath))
                    throw new InvalidOperationException("Rules file path is not configured.");

                string rulesJson = File.ReadAllText(rulesFilePath);
                Workflow workflow = JsonSerializer.Deserialize<Workflow>(rulesJson);
                if (workflow == null || workflow.Rules == null || !workflow.Rules.Any())
                    throw new InvalidOperationException("Deserialized workflow is null or empty.");

                return new RulesEngine.RulesEngine(new[] { workflow }, null);
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
        return await _dbContext.Units
            .Include(u => u.Employees)
            .FirstOrDefaultAsync(u => u.Id == unitId);
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
        await _dbContext.SaveChangesAsync

();
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
        builder.HasMany(u => u.Employees)
            .WithOne(e => e.Unit)
            .HasForeignKey(e => e.UnitId);

        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.Units)
            .HasForeignKey(e => e.TenantId);
    }
}
```

## **Presentation Layer**

### **Routes**
*This folder contains the endpoint implementations.*
```csharp
endpoints.MapGet("/units/tenant/{tenantId}", async (IUnitRepository unitRepository, int tenantId) =>
{
    IEnumerable<Unit> units = await unitRepository.GetAllUnitsByTenantIdAsync(tenantId);
    return Results.Ok(units);
}).WithTags("Units");
```
*You can check the rest of the routes implemented inside the Routes folder.*

## **Presentation.Api**
This is a .NET 8 web API project.

### **appsettings.json**
Here we store configuration settings for the application, including the database connection string and other key-value pairs for application behavior, environment-specific settings, and external service configurations.

### **Program.cs File**

*here we register the services and dependencies that we need in order for our application to run correctly*

```csharp
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicatinoServices(builder.Configuration);

WebApplication app = builder.Build();

app.AddRoutes();
app.AddUsings();
app.Run();
```

    