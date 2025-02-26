using System.Text.Json.Serialization;
using Bank.Common.Api.Configurations;
using Bank.Common.Api.Middlewares.Extensions;
using Bank.Core.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreSwagger();
builder.Services.AddJwtAuthentication();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseExceptionsHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();