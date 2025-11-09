using LemonTaskManagement.Api.Models;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LemonTaskManagement.Api.Controllers;

[Route("api/users/{userId:Guid}")]
[ApiController]
public class UserBoardsController(
    IUserBoardsQueryService userBoardsQueryService,
    ICardsCommandService cardsCommandService) : ControllerBase
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
    public async Task<IActionResult> GetUserBoardAsync(Guid userId, Guid boardId) =>
        Ok(await userBoardsQueryService.GetUserBoardAsync(new GetUserBoardQuery(userId, boardId)));

    [HttpPost("boards/{boardId:Guid}/cards")]
    [ProducesResponseType(201, Type = typeof(ApiResponse<CreateCardResponse>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCardAsync(Guid userId, Guid boardId, [FromBody] CreateCardCommand command)
    {
        command.UserId = userId;
        command.BoardId = boardId;

        var response = await cardsCommandService.CreateCardAsync(command);

        if (!response.Success)
        {
            return BadRequest(new ApiResponse<CreateCardResponse>(400, "Failed to create card", response));
        }

        return CreatedAtAction(nameof(CreateCardAsync), new { userId, boardId, cardId = response.Data.Id },
            new ApiResponse<CreateCardResponse>(201, "Card created successfully", response));
    }

    [HttpPut("boards/{boardId:Guid}/cards/{cardId:Guid}/move")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<MoveCardResponse>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> MoveCardAsync(Guid userId, Guid boardId, Guid cardId, [FromBody] MoveCardCommand command)
    {
        command.UserId = userId;
        command.CardId = cardId;
        command.BoardId = boardId;

        var response = await cardsCommandService.MoveCardAsync(command);

        if (!response.Success)
        {
            return BadRequest(new ApiResponse<MoveCardResponse>(400, "Failed to move card", response));
        }

        return Ok(new ApiResponse<MoveCardResponse>(200, "Card moved successfully", response));
    }
}
