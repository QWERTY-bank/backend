using Microsoft.OpenApi.Models;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddCoreSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var openApiSecurityScheme = new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your token in the text input below"
            };
            options.AddSecurityDefinition("Bearer", openApiSecurityScheme);

            var openApiSecurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new List<string>()
                }
            };
            options.AddSecurityRequirement(openApiSecurityRequirement);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName != null && x.FullName.Contains("Bank.Core"));
            
            foreach (var assembly in assemblies)
            {
                var xmlDocsPath = Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name + ".xml");

                options.IncludeXmlComments(xmlDocsPath);
            }
        });

        return services;
    }
}