using efcore.Data;

namespace efcore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _datacontext;

        public CategoryService(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public async Task<List<CategoryOutputDto>?> GetCategory(Guid request)
        {
            var category = await _datacontext.Category
                .Where(u => u.UserId == request)
                .Select(c => c.ToOutputDto())
                .ToListAsync();
            return category;
        }
        public async Task<List<CategoryOutputDto>> GetAllCategory()
        {
            return await _datacontext.Category.Select(c => c.ToOutputDto()).ToListAsync();
        }
        public async Task<CategoryOutputDto?> GetCategoryById(Guid request, Guid sender)
        {
            Category? category = await _datacontext.Category.FindAsync(request);
            if (category == null)
            {
                return null;
            }
            if (category.UserId != sender)
            {
                return null;
            }
            return category.ToOutputDto();
        }
        public async Task<string?> AddCategory(CategoryInputDto request, Guid sender)
        {
            var newCategory = new Category
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = request.CategoryName,
                CategoryDescription = request.CategoryDescription,
                UserId = sender,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _datacontext.Category.AddAsync(newCategory);
            _datacontext.SaveChanges();
            return "Category Added Successfully";
        }
        public async Task<string?> UpdateCategoryById(Guid request, CategoryInputDto newCategory, Guid sender)
        {
            Category? category = await _datacontext.Category.FindAsync(request);
            if (category == null)
            {
                return "Category not Found";
            }
            if (category.UserId != sender)
            {
                return "Unauthorized to update this category";
            }
            category.CategoryName = newCategory.CategoryName;
            category.CategoryDescription = newCategory.CategoryDescription;
            category.UpdatedAt = DateTime.Now;
            await _datacontext.SaveChangesAsync();
            return "Category Updated Successfully";
        }
        public async Task<string?> DeleteCategoryById(Guid request, Guid sender)
        {
            Category? category = await _datacontext.Category
                .FindAsync(request);
            if (category == null)
            {
                return "Category not Found";
            }
            if (category.UserId != sender)
            {
                return "Unauthorized to delete this category";
            }
            _datacontext.Category.Remove(category);
            await _datacontext.SaveChangesAsync();
            return "Category Deleted Succesfully";
        }
    }
}
