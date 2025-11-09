using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Infra.Data.Write.Seeder.SeedObjects;
using Microsoft.EntityFrameworkCore;

namespace LemonTaskManagement.Infra.Data.Write.Seeder;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(DatabaseSeederObjects.GetUsers());
        modelBuilder.Entity<Board>().HasData(DatabaseSeederObjects.GetBoards());
        modelBuilder.Entity<BoardUser>().HasData(DatabaseSeederObjects.GetBoardUsers());
        modelBuilder.Entity<BoardColumn>().HasData(DatabaseSeederObjects.GetBoardColumns());
    }
}
