namespace WordBattle.Domain.Entities.Categories
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category, CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        IQueryable<Category> GetAllAsync(CancellationToken cancellationToken);
        Task<Category?> GetRandomAsync(CancellationToken cancellationToken);
    }
}
