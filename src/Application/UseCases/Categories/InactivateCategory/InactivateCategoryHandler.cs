using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.Abstractions.UoW;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Domain.Shared;

namespace WordBattle.Application.UseCases.Categories.InactivateCategory
{
    public class InactivateCategoryHandler(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository) : ICommandHandler<InactivateCategoryCommand, Guid>
    {
        public async Task<Result<Guid>> HandleAsync(InactivateCategoryCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);
            if (category is null)
                return Result.Failure<Guid>(Error.NotFound);

            category.Inactivate();

            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(command.Id);
        }
    }
}
