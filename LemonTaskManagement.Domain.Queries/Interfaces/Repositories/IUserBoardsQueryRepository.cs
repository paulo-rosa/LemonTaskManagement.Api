using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Domain.Queries.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.Interfaces.Repositories;

public interface IUserBoardsQueryRepository
{
    Task<List<BoardUser>> GetUserBoardsAsync(GetUserBoardsQuery query);

    Task<BoardUser> GetUserBoardAsync(GetUserBoardQuery query);
}
