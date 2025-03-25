using Bank.Common.Api.Configurations;
using Bank.Common.Auth.Extensions;
using Bank.Users.Api.Configurations.Authorization;
using Bank.Users.Api.Mappers;
using Bank.Users.Application;
using Bank.Users.Application.Auth.Configurations;
using Bank.Users.Persistence;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bank.Users.Api
{
    /// <summary>
    /// Настройка зависимостей
    /// </summary>
    public static class UsersServicesExtensions
    {
        /// <summary>
        /// Добавление AutoMapper
        /// </summary>
        public static void AddApiAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthApiMapperProfile), typeof(UserApiMapperProfile));
        }

        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        public static void AddApiUsersConfigurations(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerGen(options =>
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(filePath);
            });

            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();
            services.ConfigureOptions<JwtBearerOptionsWithoutValidateLifetimeConfigure>();
            services.ConfigureOptions<AuthorizationOptionsWithoutValidateLifetimeConfigure>();
            services.ConfigureOptions<JwtServiceOptionsConfigure>();
            services.AddJwtAuthentication()
                .AddJwtBearer(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature);
        }

        /// <summary>
        /// Наполнение бд данными
        /// </summary>
        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var rolesSeed = scope.ServiceProvider.GetRequiredService<RolesSeed>();
                rolesSeed.SeedRoles();

                var usersSeed = scope.ServiceProvider.GetRequiredService<UsersSeed>();
                usersSeed.SeedUsers();
            }
        }
    }
}
