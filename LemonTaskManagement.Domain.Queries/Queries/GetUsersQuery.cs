using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUsersQuery
{
    public string NameContains { get; set; }
    public string EmailContains { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;

    public GetUsersQuery()
    {
    }

    public GetUsersQuery(string nameContains, string emailContains, int skip, int take)
    {
        NameContains = nameContains;
        EmailContains = emailContains;
        Skip = skip;
        Take = take;
    }
}

public class GetUsersResponse : Response<List<UserDto>>;
