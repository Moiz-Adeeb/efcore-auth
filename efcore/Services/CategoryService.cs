namespace efcore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public CategoryService(DataContext dataContext, IHttpContextAccessor contextAccessor)
        {
            _dataContext = dataContext;
            _contextAccessor = contextAccessor;
        }

        public async Task<List<CategoryOutputDto>?> GetCategory()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var category = await _dataContext.Category
                .Where(u => u.UserId == id)
                .AsNoTracking()
                .Select(c => new CategoryOutputDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    UserName = c.User.Username

                })
                .ToListAsync();
            return category;
        }
        public async Task<List<CategoryOutputDto>?> GetAllCategory()
        {
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            if (role is not ("Admin" or "Manager"))return null;
            return await _dataContext.Category
                .AsNoTracking()
                .Select(c => new CategoryOutputDto
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDescription,
                    UserName = c.User.Username
                
                })
                .ToListAsync();
        }
        public async Task<CategoryOutputDto?> GetCategoryById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var category = await _dataContext.Category.FindAsync(id);
            if (category == null) return null;
            return category.UserId != id ? null : category.ToOutputDto();
        }
        public async Task<string?> AddCategory(CategoryInputDto request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var newCategory = new Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                UserId = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _dataContext.Category.AddAsync(newCategory);
            await _dataContext.SaveChangesAsync();
            return "Category Added Successfully";
        }
        public async Task<string?> UpdateCategoryById(Guid request, CategoryInputDto newCategory)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var category = await _dataContext.Category.FindAsync(request);
            if (category == null) return "Category not Found";
            if (category.UserId != id) return "Unauthorized to update this category";
            category.CategoryName = newCategory.CategoryName;
            category.CategoryDescription = newCategory.CategoryDescription;
            category.UpdatedAt = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return "Category Updated Successfully";
        }
        public async Task<string?> DeleteCategoryById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var category = await _dataContext.Category.FindAsync(request);
            if (category == null) return "Category not Found";
            if (category.UserId == id || role == "Manager" || role == "Admin")
            {
                _dataContext.Category.Remove(category);
                await _dataContext.SaveChangesAsync();
                return "Category Deleted Successfully";
            } else return "Unauthorized to delete this category";
        }
    }
}
