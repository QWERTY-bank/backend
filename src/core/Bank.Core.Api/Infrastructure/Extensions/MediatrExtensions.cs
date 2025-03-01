using Bank.Core.Application.Accounts;

namespace Bank.Core.Api.Infrastructure.Extensions;

internal static class MediatrExtensions
{
    public static IServiceCollection AddCoreMediatR(this IServiceCollection services)
    {
        services.AddMediatR(
            cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(CreateAccountCommandHandler).Assembly);
            }
        );
        
        return services;
    }
}