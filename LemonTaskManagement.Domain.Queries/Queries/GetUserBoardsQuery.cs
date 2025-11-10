using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.DTOs;
using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Domain.Queries.Queries;

public class GetUserBoardsQuery
{
    public Guid UserId { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;

    public GetUserBoardsQuery()
    {
    }

    public GetUserBoardsQuery(Guid userId, int skip, int take)
    {
        UserId = userId;
        Skip = skip;
        Take = take;
    }
}

public class GetUserBoardsResponse : Response<List<UserBoardDto>>;
