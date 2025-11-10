using System;
using LemonTaskManagement.Domain.Core.Models;

namespace LemonTaskManagement.Domain.Commands.Commands;

public class LoginCommand
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResponse : Response
{
    public LoginDto Data { get; set; }
}

public class LoginDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}
