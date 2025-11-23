using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WordBattle.Domain.Entities.Games.Players;
using WordBattle.Infrastructure.Persistence.Configurations.Base;

namespace WordBattle.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : BaseEntityConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            base.Configure(builder);

            builder.ToTable("players");

            builder.Property(p => p.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Sequence)
                .HasColumnName("sequence")
                .IsRequired();

            builder.Property(p => p.IsPlaying)
                .HasColumnName("is_playing")
                .IsRequired();

            builder.Property(p => p.EliminatedAt)
                .HasColumnName("eliminated_at");
        }
    }
}
