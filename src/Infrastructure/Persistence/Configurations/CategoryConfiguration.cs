using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Infrastructure.Persistence.Configurations.Base;

namespace WordBattle.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.ToTable("categories");

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Active)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
