using Bank.Common.Api.Configurations;
using Bank.Common.Auth.Extensions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bank.Users.Api
{
    /// <summary>
    /// Настройка зависимостей
    /// </summary>
    public static class CreditServicesExtensions
    {
        /// <summary>
        /// Регистрация зависимостей
        /// </summary>
        public static void AddCreditServices(this IServiceCollection services)
        {

        }

        /// <summary>
        /// Добавление AutoMapper
        /// </summary>
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            //services.AddAutoMapper(
            //    typeof(AuthApplicationMapperProfile), typeof(AuthApiMapperProfile),
            //);
        }

        /// <summary>
        /// Конфигурация приложения
        /// </summary>
        public static void AddCreditConfigurations(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.IncludeXmlComments(filePath);
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();
            services.AddJwtAuthentication();
        }

        /// <summary>
        /// Настройка контекста БД
        /// </summary>
        public static void AddCreditDbContext(this WebApplicationBuilder builder)
        {
            //string? postgreConnectionString = builder.Configuration.GetConnectionString("PostgreConnection");
            //builder.Services.AddDbContext<UsersDbContext>(options => options.UseNpgsql(postgreConnectionString!));
        }

        /// <summary>
        /// Применить миграции при старте
        /// </summary>
        public static void AddAutoMigration(this IServiceProvider services)
        {
            //using var dbContext = services.CreateScope().ServiceProvider.GetRequiredService<UsersDbContext>();
            //dbContext.Database.Migrate();
        }
    }
}
