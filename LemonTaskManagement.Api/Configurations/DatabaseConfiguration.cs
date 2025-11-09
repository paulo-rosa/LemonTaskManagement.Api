using LemonTaskManagement.Infra.Data.Context;
using LemonTaskManagement.Infra.Data.Read.Context;
using Microsoft.EntityFrameworkCore;

namespace LemonTaskManagement.Api.Configurations;

public static class DatabaseConfiguration
{
    private const string DataWriteProjectName = "LemonTaskManagement.Infra.Data.Write";
    private const string DataReadProjectName = "LemonTaskManagement.Infra.Data.Read";

    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var useInMemoryDatabase = configuration.GetValue("Database:UseInMemory", true);

        services.AddDbContext<LemonTaskManagementDbContext>(options =>
        {
            if (useInMemoryDatabase)
            {
                options.UseInMemoryDatabase("LemonTaskManagementDb");
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsAssembly(DataWriteProjectName));
            }
        });

        services.AddDbContext<LemonTaskManagementReadOnlyDbContext>(options =>
        {
            if (useInMemoryDatabase)
            {
                options.UseInMemoryDatabase("LemonTaskManagementDb");
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.MigrationsAssembly(DataReadProjectName));
            }
        });

        services.AddScoped<ILemonTaskManagementDbContext>(provider =>
            provider.GetRequiredService<LemonTaskManagementDbContext>());
    }

    public static void UseDatabase(this IApplicationBuilder app, IConfiguration configuration)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope == null) return;

        var context = serviceScope.ServiceProvider.GetRequiredService<LemonTaskManagementDbContext>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<LemonTaskManagementDbContext>>();

        try
        {
            var useInMemoryDatabase = configuration.GetValue("Database:UseInMemory", true);

            if (useInMemoryDatabase)
            {
                context.Database.EnsureCreated();
            }
            else
            {
                logger.LogInformation("Applying database migrations...");
                context.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully (including seed data).");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}
