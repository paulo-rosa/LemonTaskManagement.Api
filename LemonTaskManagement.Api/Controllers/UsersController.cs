using LemonTaskManagement.Api.Models;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonTaskManagement.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController(IUsersQueryService userQueryService) : ControllerBase
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUserResponse>))]
    public async Task<IActionResult> GetUserAsync(Guid id) => Ok(await userQueryService.GetUserAsync(new GetUserQuery(id)));

    [HttpGet("")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUsersResponse>))]
    public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsersQuery query) => Ok(await userQueryService.GetUsersAsync(query));
}
