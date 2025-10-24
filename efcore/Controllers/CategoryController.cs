using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<List<CategoryOutputDto>?> GetCategory()
        {
            return await _categoryService.GetCategory();
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<CategoryOutputDto>?> GetAllCategory()
        {
            return await _categoryService.GetAllCategory();
        }
        [HttpGet("getcategory")]
        [Authorize(Roles = "User")]
        public async Task<CategoryOutputDto?> GetCategoryById(Guid request)
        {
            return await _categoryService.GetCategoryById(request);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<string?> AddCategory(CategoryInputDto request)
        {
            return await _categoryService.AddCategory(request);
        }
        [HttpPut($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User")]
        public async Task<string?> UpdateCategoryById(Guid request, CategoryInputDto newCategory)
        {
            return await _categoryService.UpdateCategoryById(request, newCategory);
        }

        [HttpDelete($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User, Admin, Manager")]
        public async Task<string?> DeleteCategoryById(Guid request)
        {
            return await _categoryService.DeleteCategoryById(request);
        }
    }
}
