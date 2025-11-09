using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.Queries;
using LemonTaskManagement.Infra.Data.Read.Context;
using LemonTaskManagement.Infra.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonTaskManagement.Infra.Data.Read;

public class UserBoardsQueryRepository(LemonTaskManagementReadOnlyDbContext context) : QueryRepository<BoardUser>(context), IUserBoardsQueryRepository
{
    public async Task<List<BoardUser>> GetUserBoardsAsync(GetUserBoardsQuery query) =>
        await DbEntity
            .AsNoTracking()
            .Where(u => u.UserId == query.UserId)
            .Include(u => u.User)
            .Include(u => u.Board)
            .Skip(query.Skip)
            .Take(query.Take)
            .ToListAsync();

    public async Task<BoardUser> GetUserBoardAsync(GetUserBoardQuery query) =>
        await DbEntity
            .AsNoTracking()
            .Include(u => u.User)
            .Include(u => u.Board)
            .FirstOrDefaultAsync(u => u.BoardId == query.BoardId && u.UserId == query.UserId);
}
