using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Errors;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Games.StartNextRound
{
    public class StartNextRoundHandler(IUnitOfWork unitOfWork, IGameRepository
        gameRepository, ICategoryRepository categoryRepository) : ICommandHandler<StartNextRoundCommand, StartNextRoundResponse>
    {
        public async Task<Result<StartNextRoundResponse>> HandleAsync(StartNextRoundCommand command, CancellationToken cancellationToken = default)
        {
            var game = await gameRepository.GetByIdAsync(command.gameId, cancellationToken);
            if (game is null)
                return Result.Failure<StartNextRoundResponse>(Error.NotFound);

            var randomCategory = await categoryRepository.GetRandomAsync(cancellationToken);
            if (randomCategory is null)
                return Result.Failure<StartNextRoundResponse>(DomainErrors.Category.NoActiveCategory);

            var result = game.StartNextRound(randomCategory.Id);
            if (result.IsFailure)
                return Result.Failure<StartNextRoundResponse>(result.Error);

            await unitOfWork.CommitAsync(cancellationToken);

            return new StartNextRoundResponse(
                game.CurrentRound!.Id,
                game.CurrentRound!.Number,
                randomCategory.Description,
                game.GetFirstActivePlayer()!.Name,
                game.GetFirstActivePlayer()!.Id
             );
        }
    }
}
