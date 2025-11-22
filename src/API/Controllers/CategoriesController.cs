using Microsoft.AspNetCore.Mvc;
using WordBattle.API.Controllers.Abstractions;
using WordBattle.Application.Abstractions.Messaging;
using WordBattle.Application.UseCases.Categories.CreateCategory;

namespace WordBattle.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController(ICommandHandler<CreateCategoryCommand, Guid> createCategoryUseCase) : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            return await HandleResult(async () => await createCategoryUseCase.HandleAsync(command, cancellationToken));
        }
    }
}
