using Bank.Common.Api.Configurations;
using Bank.Users.Api;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureAppsettings();
builder.Services.AddUsersConfigurations();
builder.Services.AddUsersServices();
builder.Services.AddAutoMapperProfiles();
builder.AddUsersDbContext();
builder.AddRedisDb();

var app = builder.Build();

app.Services.AddAutoMigration();
app.Services.AddDatabaseSeed();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();