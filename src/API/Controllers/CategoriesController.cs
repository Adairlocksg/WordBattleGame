using Microsoft.AspNetCore.Mvc;

namespace WordBattle.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return Ok();
        }
    }
}
