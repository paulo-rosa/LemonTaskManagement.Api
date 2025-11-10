using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using LemonTaskManagement.Domain.Commands.Interfaces.Services;
using LemonTaskManagement.Domain.Core.Models;
using LemonTaskManagement.Domain.Queries.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Commands.CommandServices;

public class AuthenticationCommandService : IAuthenticationCommandService
{
    private readonly IUsersQueryRepository _usersQueryRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationCommandService(
        IUsersQueryRepository usersQueryRepository,
        IJwtTokenService jwtTokenService)
    {
        _usersQueryRepository = usersQueryRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponse> LoginAsync(LoginCommand command)
    {
        var errors = new List<Error>();

        // Validate input
        if (string.IsNullOrWhiteSpace(command.Username))
        {
            errors.Add(new Error("Username", "Username is required"));
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            errors.Add(new Error("Password", "Password is required"));
        }

        if (errors.Count > 0)
        {
            return new LoginResponse
            {
                Success = false,
                Errors = errors
            };
        }

        // Get user by username
        var user = await _usersQueryRepository.GetUserByUsernameAsync(command.Username);
        if (user == null)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password",
                Errors = new List<Error> { new Error("Authentication", "Invalid username or password") }
            };
        }

        // Verify password using BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(command.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return new LoginResponse
            {
                Success = false,
                Message = "Invalid username or password",
                Errors = new List<Error> { new Error("Authentication", "Invalid username or password") }
            };
        }

        // Generate JWT token
        var token = _jwtTokenService.GenerateToken(user);
        var expiresAt = _jwtTokenService.GetTokenExpiration();

        return new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Data = new LoginDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token,
                ExpiresAt = expiresAt
            }
        };
    }
}
