using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Credits.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsConfigure();

builder.ConfigureAppsettings();
builder.Services.AddCreditConfigurations();
builder.Services.AddCreditServices();
builder.Services.AddAutoMapperProfiles();
builder.AddCreditDbContext();

var app = builder.Build();

app.Services.AddAutoMigration();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();