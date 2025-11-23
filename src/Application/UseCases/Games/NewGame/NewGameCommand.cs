using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Games.NewGame
{
    public record NewGameCommand : ICommand<Guid>
    {
    }
}
