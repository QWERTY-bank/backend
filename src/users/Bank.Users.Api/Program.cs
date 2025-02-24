using Bank.Common.Api.Configurations;
using Bank.Users.Api.Configurations.Authorization;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    options.IncludeXmlComments(filePath);
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Configuration extensions
builder.Services.AddSwaggerConfigure();
builder.Services.AddModalStateConfigure();
builder.Services.ConfigureOptions<JwtBearerOptionsWithoutValidateLifetimeConfigure>();
builder.Services.ConfigureOptions<AuthorizationOptionsWithoutValidateLifetimeConfigure>();
builder.Services.AddJwtAuthentication()
    .AddJwtBearer(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();