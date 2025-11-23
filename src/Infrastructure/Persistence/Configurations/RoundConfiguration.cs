using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WordBattle.Domain.Entities.Games.Rounds;
using WordBattle.Infrastructure.Persistence.Configurations.Base;

namespace WordBattle.Infrastructure.Persistence.Configurations
{
    public class RoundConfiguration : BaseEntityConfiguration<Round>
    {
        public override void Configure(EntityTypeBuilder<Round> builder)
        {
            base.Configure(builder);

            builder.ToTable("rounds");

            builder.Property(r => r.Number)
                .HasColumnName("number")
                .IsRequired();
            builder.Property(r => r.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();
            builder.Property(r => r.CurrentTurnStartedAt)
                .HasColumnName("current_turn_started_at")
                .IsRequired();
            builder.Property(r => r.IsFinished)
                .HasColumnName("is_finished")
                .IsRequired();

            builder.Property(r => r.ExpectedAnswers)
                .HasColumnName("expected_answers")
                .IsRequired();

            builder.HasMany(r => r.Answers)
                   .WithOne()
                   .HasForeignKey(x => x.RoundId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
