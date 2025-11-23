using Microsoft.EntityFrameworkCore;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Infrastructure.Persistence;

namespace WordBattle.Infrastructure.Repositories
{
    public class CategoryRepository(WordBattleDbContext dbContext) : ICategoryRepository
    {
        private readonly DbSet<Category> DbSet = dbContext.Set<Category>();

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(category, cancellationToken);
        }

        public IQueryable<Category> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return DbSet.Where(x => x.Active);
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return dbContext.Categories.FindAsync([id], cancellationToken).AsTask();
        }

        public Task<Category?> GetRandomAsync(CancellationToken cancellationToken = default)
        {
            return DbSet.OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync(x => x.Active, cancellationToken);
        }
    }
}
