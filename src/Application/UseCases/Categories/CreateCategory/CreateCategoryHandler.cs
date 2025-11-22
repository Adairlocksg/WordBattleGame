using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Categories.CreateCategory
{
    public class CreateCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : ICommandHandler<CreateCategoryCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            var categoryResult = Category.Create(command.Description);

            if (categoryResult.IsFailure)
                return Result.Failure<Guid>(categoryResult.Error);

            var category = categoryResult.Value;

            await categoryRepository.AddAsync(category, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(category.Id);
        }
    }
}
