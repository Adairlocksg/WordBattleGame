using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Errors;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Games.AddPlayer
{
    public class AddPlayerHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository) : ICommandHandler<AddPlayerCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(AddPlayerCommand command, CancellationToken cancellationToken)
        {
            var game = await gameRepository.GetByIdAsync(command.GameId, cancellationToken);
            if (game is null)
                return Result.Failure<Guid>(Error.NotFound);

            var addPlayerResult = game.AddPlayer(command.PlayerName);
            if (addPlayerResult.IsFailure)
                return Result.Failure<Guid>(addPlayerResult.Error);

            await gameRepository.UpdateAsync(game, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(game.Id);
        }
    }
}
