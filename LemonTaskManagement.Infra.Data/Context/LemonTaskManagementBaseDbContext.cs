using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LemonTaskManagement.Infra.Data.Context;

public class LemonTaskManagementBaseDbContext(DbContextOptions options) : DbContext(options), ILemonTaskManagementDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardUser> BoardUsers { get; set; }
    public DbSet<BoardColumn> BoardColumns { get; set; }
    public DbSet<Card> Cards { get; set; }

    protected string GetCreatedBy()
    {
        //TODO - Inject IUserClaimsProvider and IEnvironmentService to get actual values
        //if (!string.IsNullOrEmpty(userClaimsProvider?.UserId))
        //    return userClaimsProvider.UserId;

        //return !string.IsNullOrEmpty(environmentService?.ApplicationName)
        //    ? environmentService.ApplicationName
        //    : ApiConstants.Name;

        return "System";
    }

    protected static void SetDefaultConfiguration(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected static void ApplyAllConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LemonTaskManagementBaseDbContext).Assembly);
    }
}

