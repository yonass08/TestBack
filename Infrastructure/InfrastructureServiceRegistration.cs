using Application.Contracts.Infrastructure;
using Infrastructure.Chat;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;


public static class InfrastructureServiceRegistration
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddTransient<IChatRequestSender, ChatRequestSender>();
        return services;
    }

}