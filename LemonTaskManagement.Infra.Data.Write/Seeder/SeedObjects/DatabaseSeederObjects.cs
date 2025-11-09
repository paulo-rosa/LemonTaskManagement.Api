using LemonTaskManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace LemonTaskManagement.Infra.Data.Write.Seeder.SeedObjects;

public static class DatabaseSeederObjects
{

    private static DateTimeOffset CreatedDate => new(2025, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
    private static string CreatedBy => "Seed";

    private static readonly Guid user1Id = new Guid("442c1c9d-8a84-40ed-bf71-b7de2dde2f86");
    private static readonly Guid user2Id = new Guid("a5dc03d3-e1dd-4800-8883-71f33d9c1af5");
    private static readonly Guid user3Id = new Guid("5e58391c-a427-44a8-9047-17c7657e3473");

    public static List<User> GetUsers()
    {

        var users = new List<User>
        {
            new User
            {
                Id = user1Id,
                Username = "john.doe",
                Email = "john.doe@example.com",
                PasswordHash = "$2a$11$vKzN8p5XJ5kKZX8Xp8xvXeYZJ5X8X8X8X8X8X8X8X8X8X8X8X8X8Xu", // Password: Password123!
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new User
            {
                Id = user2Id,
                Username = "jane.smith",
                Email = "jane.smith@example.com",
                PasswordHash = "$2a$11$vKzN8p5XJ5kKZX8Xp8xvXeYZJ5X8X8X8X8X8X8X8X8X8X8X8X8X8Xu", // Password: Password123!
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new User
            {
                Id = user3Id,
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = "$2a$11$wLaN9q6YK6lLaY9Yq9ywYfZaK6Y9Y9Y9Y9Y9Y9Y9Y9Y9Y9Y9Y9Y9Yv", // Password: Admin123!
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            }
        };

        return users;
    }

    private static readonly Guid board1Id = new Guid("362ccd7c-d795-4156-aa52-de838ac494b1");
    private static readonly Guid board2Id = new Guid("7b429576-c964-4c7c-bcee-b494b274c920");
    private static readonly Guid board3Id = new Guid("58c35ceb-fe5e-4b6e-8f66-e8560505fd60");
    private static readonly Guid board4Id = new Guid("5002e0a6-9e12-4efb-aa18-8b44edbfd956");

    public static List<Board> GetBoards()
    {

        var boards = new List<Board>
        {
            new Board
            {
                Id = board1Id,
                Name = "Development Tasks",
                Description = "Track all development related tasks and features",
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new Board
            {
                Id = board2Id,
                Name = "Marketing Campaign",
                Description = "Plan and execute marketing campaigns",
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new Board
            {
                Id = board3Id,
                Name = "Personal Projects",
                Description = "My personal side projects and ideas",
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new Board
            {
                Id = board4Id,
                Name = "Team Collaboration",
                Description = "Shared board for team tasks and discussions",
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            }
        };

        return boards;
    }

    public static List<BoardUser> GetBoardUsers()
    {
        var users = GetUsers();
        var boards = GetBoards();
        var boardUsers = new List<BoardUser>
        {            
            // John Doe boards
            new BoardUser
            {
                UserId = user1Id,
                BoardId = board1Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new BoardUser
            {
                UserId = user1Id,
                BoardId = board4Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            // Jane Smith boards
            new BoardUser
            {
                UserId = user2Id,
                BoardId = board2Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new BoardUser
            {
                UserId = user2Id,
                BoardId = board4Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            // Admin boards (has access to all)
            new BoardUser
            {
                UserId = user3Id,
                BoardId = board1Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new BoardUser
            {
                UserId = user3Id,
                BoardId = board2Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new BoardUser
            {
                UserId = user3Id,
                BoardId = board3Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            },
            new BoardUser
            {
                UserId = user3Id,
                BoardId = board4Id,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            }

        };
        return boardUsers;
    }

    private static readonly Guid[][] boardColumnIds = {
        [
            new("68d035b7-fa02-4213-9f31-64567035571a"),
            new("72b28290-9841-45f9-917d-c81c7349f46e"),
            new("76b126d9-a102-4080-af13-5ef926cbd772")
        ],
        [
            new("a8338f5e-69af-4869-89ce-c2005f709362"),
            new("ce515d30-76bc-45f7-8292-435368e2fc19"),
            new("2b598d88-5afd-4f27-a538-956abf35b115"),
        ],
        [
            new("fd3de6ba-ff1e-4fe9-bb2b-4acb5186e38d"),
            new("0188a65a-06cc-43e6-8c13-ee93aa586348"),
            new("06a72d5e-6319-4795-b036-2739acf5051e")
        ],
        [
            new("a891bed6-9dba-42ec-a811-646eec934753"),
            new("e0ee4e9e-9097-422f-801b-c77fd3211151"),
            new("fbe50e15-f74b-4735-94b1-15bc3f83c05b")
        ]
    };

    public static List<BoardColumn> GetBoardColumns()
    {
        var boards = GetBoards();
        var boardColumns = new List<BoardColumn>();

        var counter = 0;

        // Create default columns (TO-DO, DOING, DONE) for each board
        foreach (var board in boards)
        {
            var currentBoardIdArray = boardColumnIds[counter];

            boardColumns.Add(new BoardColumn
            {
                Id = currentBoardIdArray[0],
                BoardId = board.Id,
                Name = "TO-DO",
                Order = 1,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            });

            boardColumns.Add(new BoardColumn
            {
                Id = currentBoardIdArray[1],
                BoardId = board.Id,
                Name = "DOING",
                Order = 2,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            });

            boardColumns.Add(new BoardColumn
            {
                Id = currentBoardIdArray[2],
                BoardId = board.Id,
                Name = "DONE",
                Order = 3,
                CreatedAt = CreatedDate,
                CreatedBy = CreatedBy,
                UpdatedAt = CreatedDate,
                UpdatedBy = CreatedBy
            });

            counter++;
        }

        return boardColumns;
    }
}

