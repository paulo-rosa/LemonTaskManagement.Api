using LemonTaskManagement.Domain.Entities;
using LemonTaskManagement.Domain.Queries.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.Interfaces.Repositories;

public interface IUsersQueryRepository
{
    Task<List<User>> GetUsersAsync(GetUsersQuery query);

    Task<User> GetUserAsync(GetUserQuery query);
}
