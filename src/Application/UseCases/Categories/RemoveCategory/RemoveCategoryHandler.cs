using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Categories.RemoveCategory
{
    public class RemoveCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : ICommandHandler<RemoveCategoryCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(RemoveCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);
            if (category is null)
                return Result.Failure<Guid>(Error.NotFound);

            await categoryRepository.RemoveAsync(category, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(command.Id);
        }
    }
}
