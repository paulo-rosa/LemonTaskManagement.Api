using LemonTaskManagement.Domain.Queries.Queries;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;

public interface IUsersQueryService
{
    Task<GetUserResponse> GetUserAsync(GetUserQuery query);
    Task<GetUsersResponse> GetUsersAsync(GetUsersQuery query);
}
