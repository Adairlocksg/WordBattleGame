using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WordBattle.Domain.Entities.Games;
using WordBattle.Infrastructure.Persistence.Configurations.Base;

namespace WordBattle.Infrastructure.Persistence.Configurations
{
    public class GameConfiguration: BaseEntityConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            base.Configure(builder);

            builder.ToTable("games");

            builder.Property(g => g.StartedAt)
                .HasColumnName("started_at");

            builder.Property(g => g.FinishedAt)
                .HasColumnName("finished_at");

            builder.Property(g => g.WinnerPlayerId)
                .HasColumnName("winnerplayer_id");

            builder.HasMany(x => x.Players)
                .WithOne()
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Rounds)
                .WithOne()
                .HasForeignKey(x => x.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
