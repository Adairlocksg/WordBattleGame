namespace WordBattle.Application.Abstractions.UoW
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
