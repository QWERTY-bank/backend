using Bank.Users.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddUsersConfigurations();
builder.Services.AddUsersServices();
builder.AddUsersDbContext();

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