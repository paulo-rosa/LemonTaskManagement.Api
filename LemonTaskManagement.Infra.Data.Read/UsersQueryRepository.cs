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

public class UsersQueryRepository(LemonTaskManagementReadOnlyDbContext context) : QueryRepository<User>(context), IUsersQueryRepository
{
    public async Task<List<User>> GetUsersAsync(GetUsersQuery query) =>
        await DbEntity
            .AsNoTracking()
            .Where(u => (string.IsNullOrEmpty(query.NameContains) || u.Username.Contains(query.NameContains)) &&
                        (string.IsNullOrEmpty(query.EmailContains) || u.Email.Contains(query.EmailContains)))
            .Skip(query.Skip)
            .Take(query.Take)
            .ToListAsync();

    public async Task<User> GetUserAsync(GetUserQuery query) =>
        await DbEntity
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == query.Id);
}