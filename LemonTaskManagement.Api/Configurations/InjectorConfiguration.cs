using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.QueryServices;
using LemonTaskManagement.Infra.Data.Read;

namespace LemonTaskManagement.Api.Configurations;

internal static class InjectorConfiguration
{
    public static void ConfigureInjector(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.InjectorConfigurationQueries(configuration, environment);
    }

    private static void InjectorConfigurationCommands(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
    }

    private static void InjectorConfigurationQueries(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<IUsersQueryService, UsersQueryService>();
        services.AddScoped<IUsersQueryRepository, UsersQueryRepository>();
    }
}
