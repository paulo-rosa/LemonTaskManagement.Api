using LemonTaskManagement.Domain.Queries.DTOs;
using LemonTaskManagement.Domain.Queries.Interfaces.QueryServices;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using LemonTaskManagement.Domain.Queries.Queries;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Queries.QueryServices;

public class UserBoardsQueryService(IUserBoardsQueryRepository userBoardsQueryRepository) : IUserBoardsQueryService
{
    public Task<GetUserBoardResponse> GetUserBoardAsync(GetUserBoardQuery query)
    {
        return userBoardsQueryRepository.GetUserBoardAsync(query)
            .ContinueWith(task => new GetUserBoardResponse
            {
                Data = new UserBoardDto
                {
                    BoardId = task.Result.BoardId,
                    Board = new BoardDto
                    {
                        Id = task.Result.Board.Id,
                        Name = task.Result.Board.Name,
                        Description = task.Result.Board.Description
                    },
                    UserId = task.Result.UserId,
                    User = new UserDto
                    {
                        Id = task.Result.User.Id,
                        Username = task.Result.User.Username,
                        Email = task.Result.User.Email
                    },
                    CreatedAt = task.Result.CreatedAt
                }
            });
    }

    public async Task<GetUserBoardsResponse> GetUserBoardsAsync(GetUserBoardsQuery query)
    {
        var user = await userBoardsQueryRepository.GetUserBoardsAsync(query);

        return new GetUserBoardsResponse
        {
            Data = user.ConvertAll(boardUser => new UserBoardDto
            {
                BoardId = boardUser.BoardId,
                Board = new BoardDto
                {
                    Id = boardUser.Board.Id,
                    Name = boardUser.Board.Name,
                    Description = boardUser.Board.Description
                },
                UserId = boardUser.UserId,
                User = new UserDto
                {
                    Id = boardUser.User.Id,
                    Username = boardUser.User.Username,
                    Email = boardUser.User.Email
                },
                CreatedAt = boardUser.CreatedAt
            })
        };
    }
}
