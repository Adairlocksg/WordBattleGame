using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Games.AddPlayer
{
    public record AddPlayerCommand(Guid GameId, string PlayerName) : ICommand<Guid>;
}
