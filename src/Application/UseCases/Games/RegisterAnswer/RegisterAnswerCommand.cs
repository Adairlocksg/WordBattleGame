using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Domain.ValueObjects;

namespace WordBattle.Application.UseCases.Games.RegisterAnswer
{
    public record RegisterAnswerCommand(Guid GameId, Guid PlayerId, string Word) : ICommand<AnswerResult>;
}
