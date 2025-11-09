using LemonTaskManagement.Domain.Commands.CommandServices;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using LemonTaskManagement.Domain.Commands.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.QueryServices;
using LemonTaskManagement.Infra.Data.Read;
using LemonTaskManagement.Infra.Data.Write.Repositories;

namespace LemonTaskManagement.Api.Configurations;

internal static class InjectorConfiguration
{
    public static void ConfigureInjector(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        services.InjectorConfigurationCommands(configuration, environment);
        services.InjectorConfigurationQueries(configuration, environment);
    }

    private static void InjectorConfigurationCommands(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<ICardsCommandRepository, CardsCommandRepository>();
        services.AddScoped<ICardsCommandService, CardsCommandService>();
    }

    private static void InjectorConfigurationQueries(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<IUsersQueryRepository, UsersQueryRepository>();
        services.AddScoped<IUsersQueryService, UsersQueryService>();
        services.AddScoped<IUserBoardsQueryRepository, UserBoardsQueryRepository>();
        services.AddScoped<IUserBoardsQueryService, UserBoardsQueryService>();
    }
}
