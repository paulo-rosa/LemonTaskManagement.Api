using LemonTaskManagement.Api.Models;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LemonTaskManagement.Api.Controllers;

[Route("api/users/{userId:Guid}")]
[ApiController]
public class UserBoardController(IUserBoardsQueryService userBoardsQueryService) : ControllerBase
{

    [HttpGet("boards")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUsersResponse>))]
    public async Task<IActionResult> GetUserBoardsAsync([FromRoute] Guid userId, [FromQuery] GetUserBoardsQuery query)
    {
        query.UserId = userId;
        return Ok(await userBoardsQueryService.GetUserBoardsAsync(query));
    }

    [HttpGet("boards/{boardId:Guid}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUserResponse>))]
    public async Task<IActionResult> GetUserBoardAsync(Guid userId, Guid boardId) => Ok(await userBoardsQueryService.GetUserBoardAsync(new GetUserBoardQuery(userId, boardId)));
}
