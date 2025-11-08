using LemonTaskManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Context
{
    public interface ILemonTaskManagementDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        DbSet<User> Users { get; set; }
        DbSet<Board> Boards { get; set; }
        DbSet<UserBoard> UserBoards { get; set; }
    }
}
