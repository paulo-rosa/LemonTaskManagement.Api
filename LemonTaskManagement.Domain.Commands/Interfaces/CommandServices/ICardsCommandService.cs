using LemonTaskManagement.Domain.Commands.Commands;
using System.Threading.Tasks;

namespace LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;

public interface ICardsCommandService
{
    Task<CreateCardResponse> CreateCardAsync(CreateCardCommand command);
    Task<MoveCardResponse> MoveCardAsync(MoveCardCommand command);
    Task<UpdateCardResponse> UpdateCardAsync(UpdateCardCommand command);
}
