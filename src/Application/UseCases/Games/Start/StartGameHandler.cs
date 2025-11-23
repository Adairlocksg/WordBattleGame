using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Games.Start
{
    public class StartGameHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository) : ICommandHandler<StartGameCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(StartGameCommand command, CancellationToken cancellationToken = default)
        {
            var game = await gameRepository.GetByIdAsync(command.GameId, cancellationToken);
            if (game is null)
                return Result.Failure<Guid>(Error.NotFound);

            var result = game.StartGame();
            if (result.IsFailure)
                return Result.Failure<Guid>(result.Error);

            await unitOfWork.CommitAsync(cancellationToken);

            return game.Id;
        }
    }
}
