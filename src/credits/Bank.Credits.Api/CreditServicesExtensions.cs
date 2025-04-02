using Bank.Common.Api.Configurations;
using Bank.Common.Auth.Extensions;
using Bank.Common.Kafka;
using Bank.Common.Kafka.Transfers;
using Bank.Core.Api.Infrastructure.Extensions.Kafka;
using Bank.Credits.Api.Mappers;
using Bank.Credits.Application.Credits;
using Bank.Credits.Application.Credits.Mapper;
using Bank.Credits.Application.Requests;
using Bank.Credits.Application.Requests.Configurations;
using Bank.Credits.Application.Tariffs;
using Bank.Credits.Application.Tariffs.Mapper;
using Bank.Credits.Application.User;
using Bank.Credits.Application.User.Mapper;
using Bank.Credits.Kafka;
using Bank.Credits.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Bank.Credits.Api
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
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICreditsService, CreditsService>();
            services.AddScoped<ITariffsService, TariffsService>();

            services.AddScoped<ICoreRequestService, CoreRequestService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddMemoryCache();
        }

        public static void AddCoreKafka(this WebApplicationBuilder builder)
        {
            var kafkaSettings = builder.Configuration.GetRequiredSection("KafkaSettings").Get<KafkaSettings>()!;

            builder.Services.AddConsumer<TransferResponse, TransferConsumer>(
                kafkaSettings.BootstrapServers,
                kafkaSettings.TransferConsumer.TopicName,
                kafkaSettings.TransferConsumer.ConsumerGroup);

            builder.Services.AddProducer<UnitAccountTransferModel>(
                kafkaSettings.BootstrapServers,
                kafkaSettings.TransferRequestProducer.TopicName);
        }

        /// <summary>
        /// Добавление AutoMapper
        /// </summary>
        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(CreditApplicationMapperProfile), typeof(CreditApiMapperProfile),
                typeof(TariffApplicationMapperProfile), typeof(TariffApiMapperProfile),
                typeof(UserApplicationMapperProfile)
            );
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

            services.ConfigureOptions<TokenServiceOptionsConfigure>();
            services.ConfigureOptions<CoreRequestOptionsConfigure>();
        }

        /// <summary>
        /// Настройка контекста БД
        /// </summary>
        public static void AddCreditDbContext(this WebApplicationBuilder builder)
        {
            string? postgreConnectionString = builder.Configuration.GetConnectionString("PostgreConnection");
            builder.Services.AddDbContext<CreditsDbContext>(options => options.UseNpgsql(postgreConnectionString!));
        }

        /// <summary>
        /// Применить миграции при старте
        /// </summary>
        public static void AddAutoMigration(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<CreditsDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
