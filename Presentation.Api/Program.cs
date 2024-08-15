using Presentation.Api.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicatinoServices(builder.Configuration);

WebApplication app = builder.Build();

app.AddRoutes();
app.AddUsings();
app.Run();
