using Bank.Common.Api.Configurations;
using Bank.Common.Api.Cors;
using Bank.Common.Auth.Extensions;
using Bank.Common.OpenTelemetry;
using Bank.Users.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCorsConfigure();
builder.ConfigureAppsettings();
builder.Services.AddUsersServices();
builder.Services.AddJwtAuthentication();
builder.Services.AddApplicationAutoMapperProfiles();
builder.AddUsersDbContext();
builder.AddRedisDb();
builder.Services.AddOpenTelemetry(builder.Configuration, "authWeb");

var app = builder.Build();

app.Services.AddAutoMigration();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.UseOpenTelemetry();

app.Run();
