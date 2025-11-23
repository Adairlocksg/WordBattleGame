namespace WordBattle.Domain.Entities.Games
{
    public interface IGameRepository
    {
        Task AddAsync(Game game, CancellationToken cancellationToken);
        Task UpdateAsync(Game game, CancellationToken cancellationToken);
        Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
