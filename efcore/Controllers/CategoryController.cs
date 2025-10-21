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
        public async Task<ActionResult<List<Category>?>> GetCategory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var request = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid request))
            {
                return Ok(await _CategoryService.GetCategory(request));
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Category>>> GetAllCategory()
        {
            return Ok(await _CategoryService.GetAllCategory());
        }
        [HttpGet("getcategory")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid request)
        {
            Category? category = await _CategoryService.GetCategoryById(request);
            if (category == null)
            {
                return NotFound("Category not Found");
            }
            return Ok(category);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Category?>> AddCategory(CategoryInputDto request)
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
        public async Task<ActionResult<Category>> UpdateCategoryById(Guid request, CategoryInputDto newCategory)
        {
            var categories = await _CategoryService.UpdateCategoryById(request, newCategory);
            if (categories == null)
            {
                return NotFound("Category not Found");
            }
            return Ok(categories);
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Category>> DeleteCategoryById(Guid request)
        {
            var category = await _CategoryService.DeleteCategoryById(request);
            if (category == null)
            {
                return NotFound("Category not Found");
            }
            return Ok(category);
        }
    }
}
