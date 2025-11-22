using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Categories.CreateCategory
{
    public record CreateCategoryCommand(string Description) : ICommand<Guid>;
}
