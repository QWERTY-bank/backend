using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Common.OpenTelemetry;
using Bank.Credits.Api;
using Bank.Credits.Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfigure();

builder.ConfigureAppsettings();
builder.Services.AddCreditConfigurations();
builder.Services.AddCreditServices(builder.Configuration);
builder.Services.AddAutoMapperProfiles();
builder.Services.AddJobs(builder.Configuration);
builder.AddCreditDbContext();
builder.AddCoreKafka();
builder.AddOpenTelemetry("credits");

var app = builder.Build();

app.Services.AddAutoMigration();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseOpenTelemetry();

app.Run();