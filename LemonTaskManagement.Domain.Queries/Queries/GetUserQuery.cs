using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUserQuery
{
    public Guid Id { get; set; }

    public GetUserQuery()
    {
    }

    public GetUserQuery(Guid id)
    {
        Id = id;
    }
}

public class GetUserResponse : Response<UserDto>;
