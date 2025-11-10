using LemonTaskManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LemonTaskManagement.Infra.Data.Read.Context;

public class LemonTaskManagementReadOnlyDbContext(DbContextOptions<LemonTaskManagementReadOnlyDbContext> options) : LemonTaskManagementBaseDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder) => ApplyAllConfigurations(modelBuilder);
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => SetDefaultConfiguration(optionsBuilder);
}
