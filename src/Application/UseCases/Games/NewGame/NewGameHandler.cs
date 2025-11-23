using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Games.NewGame
{
    public class NewGameHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository) : ICommandHandler<NewGameCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(NewGameCommand command, CancellationToken cancellationToken)
        {
            var game = Game.Create();

            await gameRepository.AddAsync(game, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return game.Id;
        }
    }
}
