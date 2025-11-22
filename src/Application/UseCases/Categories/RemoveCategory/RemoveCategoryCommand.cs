using WordBattle.Application.Abstractions.Messaging;

namespace WordBattle.Application.UseCases.Categories.RemoveCategory
{
    public record RemoveCategoryCommand(Guid Id) : ICommand<Guid>;
}
