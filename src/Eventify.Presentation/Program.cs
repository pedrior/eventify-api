using Eventify.Application;
using Eventify.Infrastructure;
using Eventify.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

var app = builder.Build();

app.UsePresentation();
app.UseInfrastructure();

app.Run();