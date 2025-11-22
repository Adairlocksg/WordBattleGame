using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WordBattle.Domain.Shared;

namespace WordBattle.API.Controllers.Abstractions
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected async Task<IActionResult> HandleResult<T>(Func<Task<Result<T>>> action, string? succesMessage = null)
        {
            var result = await action();

            if (result.IsSuccess)
                return Ok(new ApiResponse<T>(true, succesMessage ?? string.Empty, result.Value, null));
            
            return BadRequest(new ApiResponse<object>(false, result.Error.Message, null, result.Error.Code));
        }        
    }
}