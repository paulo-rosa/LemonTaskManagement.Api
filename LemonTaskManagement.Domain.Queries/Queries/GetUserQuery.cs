using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUserQuery(Guid id)
{
    public Guid Id { get; set; } = id;
}

public class GetUserResponse : Response<UserDto>;
