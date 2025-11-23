using Microsoft.EntityFrameworkCore;
using WordBattle.Domain.Entities;
using WordBattle.Domain.Entities.Games;
using WordBattle.Infrastructure.Persistence;

namespace WordBattle.Infrastructure.Repositories
{
    public class GameRepository(WordBattleDbContext dbContext) : IGameRepository
    {
        protected readonly DbSet<Game> DbSet = dbContext.Set<Game>();
        public async Task AddAsync(Game game, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(game, cancellationToken);
        }

        public Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return DbSet
                .Include(x => x.Players)
                .Include(x => x.Rounds)
                    .ThenInclude(r => r.Answers)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
        }

        public Task UpdateAsync(Game game, CancellationToken cancellationToken)
        {
            DbSet.Update(game);
            return Task.CompletedTask;
        }
    }
}
