using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LemonTaskManagement.Infra.Data.Context
{
    public class LemonTaskManagementDbContext(DbContextOptions options)
        : DbContext(options), ILemonTaskManagementDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<UserBoard> UserBoards { get; set; }
    }
}
