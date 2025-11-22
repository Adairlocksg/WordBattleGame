using WordBattle.Application.Abstractions.UoW;

namespace WordBattle.Infrastructure.Persistence
{
    public class UnitOfWork(WordBattleDbContext dbContext) : IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
