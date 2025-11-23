using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Games.Start
{
    public record StartGameCommand(Guid GameId): ICommand<Guid>;
}
