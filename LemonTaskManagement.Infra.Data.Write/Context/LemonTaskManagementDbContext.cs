using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Infra.Data.Write.Seeder;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Context;

public class LemonTaskManagementDbContext(DbContextOptions<LemonTaskManagementDbContext> options) : LemonTaskManagementBaseDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyAllConfigurations(modelBuilder);
        modelBuilder.Seed();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => SetDefaultConfiguration(optionsBuilder);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var createdBy = GetCreatedBy();
        var timestamp = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not EntityBase entity) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.CreatedAt = timestamp;
                    entity.CreatedBy = createdBy;
                    break;
                case EntityState.Modified:
                    entity.UpdatedAt = timestamp;
                    entity.UpdatedBy = createdBy;
                    break;
            }
        }

        return await base.SaveChangesAsync(true, cancellationToken);
    }
}