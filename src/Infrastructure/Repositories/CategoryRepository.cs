using WordBattle.Domain.Entities.Categories;
using WordBattle.Infrastructure.Persistence;

namespace WordBattle.Infrastructure.Repositories
{
    public class CategoryRepository(WordBattleDbContext dbContext) : ICategoryRepository
    {
        public Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            dbContext.Categories.Add(category);
            return Task.CompletedTask;
        }

        public IQueryable<Category> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.Categories.AsQueryable();
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return dbContext.Categories.FindAsync([id], cancellationToken).AsTask();
        }

        public Task RemoveAsync(Category category, CancellationToken cancellationToken = default)
        {
            dbContext.Categories.Remove(category);
            return Task.CompletedTask;
        }
    }
}
