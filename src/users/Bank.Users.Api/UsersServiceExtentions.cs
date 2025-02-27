using Bank.Common.Api.Configurations;
using Bank.Common.Auth.Extensions;
using Bank.Users.Api.Configurations.Authorization;
using Bank.Users.Persistence;
using Microsoft.EntityFrameworkCore;
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
        /// Регистрация зависимостей
        /// </summary>
        public static void AddUsersServices(this IServiceCollection services)
        {

        }

        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        public static void AddUsersConfigurations(this IServiceCollection services)
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
            services.AddJwtAuthentication()
                .AddJwtBearer(JwtBearerWithoutValidateLifetimeDefaults.CheckOnlySignature);
        }

        /// <summary>
        /// Настройка контекста БД
        /// </summary>
        public static void AddUsersDbContext(this WebApplicationBuilder builder)
        {
            string? postgreConnectionString = builder.Configuration.GetConnectionString("PostgreConnection");
            builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(postgreConnectionString!));
        }

        /// <summary>
        /// Применить миграции при старте
        /// </summary>
        public static void AddAutoMigration(this IServiceProvider services)
        {
            using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<UsersDbContext>();
            dbContext.Database.Migrate();
        }

        /// <summary>
        /// Наполнение бд данными
        /// </summary>
        public static void AddDatabaseSeed(this IServiceProvider services)
        {
            using (var context = services.CreateScope().ServiceProvider.GetRequiredService<UsersDbContext>())
            {
                context.SeedData();
            }
        }
    }
}
