using efcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            this._CategoryService = CategoryService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<CategoryOutputDto>?>> GetCategory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<CategoryOutputDto>? category = await _CategoryService.GetCategory(request);
                if (category == null)
                {
                    return NotFound("Category not Found");
                }
                return Ok(category);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<CategoryOutputDto>>> GetAllCategory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<CategoryOutputDto> category = await _CategoryService.GetAllCategory();
                if (category == null)
                {
                    return NotFound("Category not Found");
                }
                return Ok(category);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getcategory")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CategoryOutputDto>> GetCategoryById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                CategoryOutputDto? category = await _CategoryService.GetCategoryById(request, sender);
                if (category == null)
                {
                    return NotFound("Category not Found");
                }
                return Ok(category);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> AddCategory(CategoryInputDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var sender = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid sender))
            {
                return Ok(await _CategoryService.AddCategory(request, sender));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpPut("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> UpdateCategoryById(Guid request, CategoryInputDto newCategory)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var categories = await _CategoryService.UpdateCategoryById(request, newCategory, sender);
                if (categories == null)
                {
                    return NotFound("Category not Found");
                }
                return Ok(categories);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> DeleteCategoryById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var categories = await _CategoryService.DeleteCategoryById(request, sender);
                if (categories == null)
                {
                    return NotFound("Category not Found");
                }
                return Ok(categories);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
    }
}
