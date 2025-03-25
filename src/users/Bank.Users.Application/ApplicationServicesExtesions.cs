using Bank.Users.Application.Auth;
using Bank.Users.Application.Auth.Mapper;
using Bank.Users.Application.Settings;
using Bank.Users.Application.Users;
using Bank.Users.Application.Users.Mapper;
using Bank.Users.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Bank.Users.Application
{
    public static class ApplicationServicesExtensions
    {
        /// <summary>
        /// Регистрация зависимостей
        /// </summary>
        public static void AddUsersServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IServiceTokenService, ServiceTokenService>();
            services.AddScoped<ITokensService, TokensService>();
            services.AddScoped<IRefreshTokenStore, RefreshTokenStore>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IClientUserSettingsService, ClientUserSettingsService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<RolesSeed>();
            services.AddScoped<UsersSeed>();
        }

        /// <summary>
        /// Добавление AutoMapper
        /// </summary>
        public static void AddApplicationAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(AuthApplicationMapperProfile), typeof(UserApplicationMapperProfile),
                typeof(ClientUserSettingsApplicationMapperProfile)
            );
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
        /// Настройка redis
        /// </summary>
        public static void AddRedisDb(this WebApplicationBuilder builder)
        {
            string? redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString!));
        }

        /// <summary>
        /// Применить миграции при старте
        /// </summary>
        public static void AddAutoMigration(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                using var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
