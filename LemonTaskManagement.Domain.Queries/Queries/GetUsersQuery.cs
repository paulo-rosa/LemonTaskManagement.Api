using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUsersQuery(string nameContains, string emailContains, int skip, int take)
{
    public string NameContains { get; set; } = nameContains;
    public string EmailContains { get; set; } = emailContains;
    public int Skip { get; set; } = skip;
    public int Take { get; set; } = take;
}

public class GetUsersResponse : Response<List<UserDto>>;
