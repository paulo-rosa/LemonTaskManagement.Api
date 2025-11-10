using LemonTaskManagement.Api.Models;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonTaskManagement.Api.Controllers;

[Route("api/users/{userId:Guid}")]
[ApiController]
[Authorize]
public class UserBoardsController(
    IUserBoardsQueryService userBoardsQueryService,
    ICardsCommandService cardsCommandService) : ControllerBase
{

    [HttpGet("boards")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUserBoardsResponse>))]
    public async Task<IActionResult> GetUserBoardsAsync([FromRoute] Guid userId, [FromQuery] GetUserBoardsQuery query)
    {
        query.UserId = userId;
        return Ok(await userBoardsQueryService.GetUserBoardsAsync(query));
    }

    [HttpGet("boards/{boardId:Guid}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<GetUserBoardResponse>))]
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

        var location = $"/api/users/{userId}/boards/{boardId}";
        return Created(location, new ApiResponse<CreateCardResponse>(201, "Card created successfully", response));
    }

    [HttpPut("boards/{boardId:Guid}/cards/{cardId:Guid}")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<UpdateCardResponse>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCardAsync(Guid userId, Guid boardId, Guid cardId, [FromBody] UpdateCardCommand command)
    {
        command.UserId = userId;
        command.BoardId = boardId;
        command.CardId = cardId;

        var response = await cardsCommandService.UpdateCardAsync(command);

        if (!response.Success)
        {
            return BadRequest(new ApiResponse<UpdateCardResponse>(400, "Failed to update card", response));
        }

        return Ok(new ApiResponse<UpdateCardResponse>(200, "Card updated successfully", response));
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
