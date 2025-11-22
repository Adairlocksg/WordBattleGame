using WordBattle.Domain.Shared;

namespace WordBattle.Application.Abstractions.Messaging
{
    public interface ICommandHandler<in TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}
