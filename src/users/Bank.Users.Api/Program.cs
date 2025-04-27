using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Common.OpenTelemetry;
using Bank.Common.Resilience;
using Bank.Users.Api;
using Bank.Users.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfigure();

builder.ConfigureAppsettings();
builder.Services.AddApiUsersConfigurations();
builder.Services.AddUsersServices();
builder.Services.AddApiAutoMapperProfiles();
builder.Services.AddApplicationAutoMapperProfiles();
builder.AddUsersDbContext();
builder.AddRedisDb();
builder.AddOpenTelemetry("users");

var app = builder.Build();

app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

app.UseErrorMiddleware();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseOpenTelemetry();

app.Run();