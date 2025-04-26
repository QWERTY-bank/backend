using System.Text.Json.Serialization;
using Bank.Common.Api.Configurations;
using Bank.Common.Api.Middlewares.Extensions;
using Bank.Common.Auth.Extensions;
using Bank.Common.OpenTelemetry;
using Bank.Core.Api.Hubs;
using Bank.Core.Api.Infrastructure.Auth;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Api.Infrastructure.Extensions.Kafka;
using Bank.Core.Api.Services;
using Bank.Core.Application.Abstractions;
using Bank.Core.Application.Common;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppsettings();
builder.Services
    .AddCoreSwagger()
    .AddCoreDatabase(builder.Configuration)
    .AddCoreKafka(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddCoreMediatR()
    .AddJwtAuthentication();

builder.Services.AddCurrencyRateClient(builder.Configuration);

builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(Policies.UnitAccount,
            policy => { policy.RequireClaim("scope", "unit-account"); });
    });

builder.Services.AddScoped<IAccountHubService, AccountHubService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<ICurrencyRateService, CurrencyRateService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOpenTelemetry(builder.Configuration, "core");

var app = builder.Build();

await app.Services.MigrateDatabaseAsync();
await app.Services.SeedDataAsync();

app.UseExceptionsHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<AccountHub>("/account-hub");
app.UseOpenTelemetry();

app.Run();