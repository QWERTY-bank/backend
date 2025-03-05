using System.Text.Json.Serialization;
using Bank.Common.Api.Configurations;
using Bank.Common.Api.Middlewares.Extensions;
using Bank.Common.Auth.Extensions;
using Bank.Core.Api.Infrastructure.Auth;
using Bank.Core.Api.Infrastructure.Extensions;
using Bank.Core.Application.Common;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppsettings();
builder.Services
    .AddCoreSwagger()
    .AddCoreDatabase(builder.Configuration)
    .AddCoreMediatR()
    .AddJwtAuthentication();

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy(Policies.UnitAccount,
            policy => { policy.RequireClaim("scope", "unit-account"); });
    });

builder.Services.AddScoped<AccountService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

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

app.Run();