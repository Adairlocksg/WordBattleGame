using Microsoft.EntityFrameworkCore;
using WordBattle.Domain.Entities;

namespace WordBattle.Infrastructure.Persistence;

public class WordBattleDbContext(DbContextOptions<WordBattleDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WordBattleDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
