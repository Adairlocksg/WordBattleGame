using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Games;
using WordBattle.Domain.Shared;
using WordBattle.Domain.ValueObjects;

namespace WordBattle.Application.UseCases.Games.RegisterAnswer
{
    public class RegisterAnswerHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository) : ICommandHandler<RegisterAnswerCommand, AnswerResult>
    {
        public async Task<Result<AnswerResult>> HandleAsync(RegisterAnswerCommand command, CancellationToken cancellationToken = default)
        {
            var game = await gameRepository.GetByIdAsync(command.GameId, cancellationToken);
            if (game is null)
                return Result.Failure<AnswerResult>(Error.NotFound);

            var result = game.RegisterAnswer(command.PlayerId, command.Word);
            if (result.IsFailure)
                return Result.Failure<AnswerResult>(result.Error);

            await unitOfWork.CommitAsync(cancellationToken);

            return result;
        }
    }
}
