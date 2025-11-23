using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Categories.InactivateCategory
{
    public record InactivateCategoryCommand(Guid Id) : ICommand<Guid>;
}
