using LemonTaskManagement.Domain.Entities;
using System;

namespace LemonTaskManagement.Domain.Commands.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    DateTimeOffset GetTokenExpiration();
}
