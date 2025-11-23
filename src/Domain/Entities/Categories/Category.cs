using WordBattle.Domain.Errors;
using WordBattle.Domain.Shared;

namespace WordBattle.Domain.Entities.Categories
{
    public class Category : Entity
    {
        internal protected Category() { }

        private Category(string description)
        {
            Description = description;
            Active = true;
        }

        public static Result<Category> Create(string description)
        {
            if (string.IsNullOrEmpty(description))
                return Result.Failure<Category>(DomainErrors.Category.EmptyDescription);

            if (description.Length < 3)
                return Result.Failure<Category>(DomainErrors.Category.ShortDescription);

            if (description.Length > 200)
                return Result.Failure<Category>(DomainErrors.Category.LongDescription);

            return new Category(description);
        }

        public void Inactivate() => Active = false;
        

        public string Description { get; private set; }
        public bool Active { get; private set; }
    }
}
