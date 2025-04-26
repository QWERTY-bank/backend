using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Common.OpenTelemetry;
using Bank.Users.Api;
using Bank.Users.Application;
//using OpenTelemetry.Exporter;
//using OpenTelemetry.Logs;
//using OpenTelemetry.Metrics;
//using OpenTelemetry.Resources;
//using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfigure();

builder.ConfigureAppsettings();
builder.Services.AddApiUsersConfigurations();
builder.Services.AddUsersServices();
builder.Services.AddApiAutoMapperProfiles();
builder.Services.AddApplicationAutoMapperProfiles();
builder.AddUsersDbContext();
builder.AddRedisDb();
builder.Services.AddOpenTelemetry(builder.Configuration, "users");

var app = builder.Build();

app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseOpenTelemetry();

app.Run();