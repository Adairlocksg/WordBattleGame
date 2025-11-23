using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Games.StartNextRound
{
    public record StartNextRoundCommand(Guid gameId) : ICommand<StartNextRoundResponse>;
}
