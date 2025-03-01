using Microsoft.OpenApi.Models;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class SwaggerExtensions
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

            var assembliesWithDocs = new [] {"Bank.Core.Domain", "Bank.Core.Application", "Bank.Core.Api"};

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName != null &&
                                   assembliesWithDocs.Any(assemblyWithDocks =>
                                       assembly.FullName.Contains(assemblyWithDocks)));
            
            foreach (var assembly in assemblies)
            {
                var xmlDocsPath = Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name + ".xml");

                options.IncludeXmlComments(xmlDocsPath);
            }
        });

        return services;
    }
}