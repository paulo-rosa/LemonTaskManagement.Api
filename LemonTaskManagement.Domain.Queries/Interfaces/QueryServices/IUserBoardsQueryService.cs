using LemonTaskManagement.Domain.Queries.Queries;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;

public interface IUserBoardsQueryService
{
    Task<GetUserBoardResponse> GetUserBoardAsync(GetUserBoardQuery query);
    Task<GetUserBoardsResponse> GetUserBoardsAsync(GetUserBoardsQuery query);
}
