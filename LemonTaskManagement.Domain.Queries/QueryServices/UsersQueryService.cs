using LemonTaskManagement.Domain.Queries.DTOs;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.Queries;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.QueryServices;

public class UsersQueryService(IUsersQueryRepository usersQueryRepository) : IUsersQueryService
{
    public async Task<GetUserResponse> GetUserAsync(GetUserQuery query)
    {
        var user = await usersQueryRepository.GetUserAsync(query);

        return new GetUserResponse
        {
            Data = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }
        };
    }

    public Task<GetUsersResponse> GetUsersAsync(GetUsersQuery query)
    {
        return usersQueryRepository.GetUsersAsync(query)
            .ContinueWith(task => new GetUsersResponse
            {
                Data = task.Result.ConvertAll(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email
                })
            });
    }
}
