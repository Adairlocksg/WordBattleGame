using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordBattle.Domain.Entities.Games.Players;
using WordBattle.Domain.Entities.Games.Rounds.Answers;

namespace WordBattle.Infrastructure.Persistence.Configurations.Base
{
    public class AnswerConfiguration: BaseEntityConfiguration<Answer>
    {
        public override void Configure(EntityTypeBuilder<Answer> builder)
        {
            base.Configure(builder);

            builder.ToTable("answers");

            builder.Property(a => a.PlayerId)
                .HasColumnName("player_id")
                .IsRequired();

            builder.Property(a => a.IsValidTime)
                .HasColumnName("is_valid_time")
                .IsRequired();

            builder.Property(a => a.IsValidContent)
                .HasColumnName("is_valid_content")
                .IsRequired();

            builder.Property(a => a.SumbitedAt)
                .HasColumnName("submited_at")
                .IsRequired();

            builder.OwnsOne(a => a.Word, wordBuilder =>
            {
                wordBuilder.Property(w => w.Value)
                           .HasColumnName("word")
                           .HasMaxLength(100)
                           .IsRequired();
            });

            builder.HasOne<Player>()
                   .WithMany()
                   .HasForeignKey(a => a.PlayerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
