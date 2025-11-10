using LemonTaskManagement.Domain.Commands.Commands;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;

public interface IAuthenticationCommandService
{
    Task<LoginResponse> LoginAsync(LoginCommand command);
}
