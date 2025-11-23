using Microsoft.AspNetCore.Mvc;
using WordBattle.API.Controllers.Abstractions;
using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.UseCases.Categories.CreateCategory;
using WordBattle.Application.UseCases.Categories.InactivateCategory;

namespace WordBattle.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController(ICommandHandler<CreateCategoryCommand, Guid> createCategoryUseCase,
        ICommandHandler<InactivateCategoryCommand, Guid> InactivateCategoryUseCase) : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            return await HandleResult(async () => await createCategoryUseCase.HandleAsync(command, cancellationToken));
        }

        [HttpPut("Inactivate")]
        public async Task<IActionResult> Inactivate([FromBody] InactivateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            return await HandleResult(async () => await InactivateCategoryUseCase.HandleAsync(command, cancellationToken));
        }
    }
}
