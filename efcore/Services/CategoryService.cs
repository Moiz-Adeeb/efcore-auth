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

        public async Task<List<Category>?> GetCategory(Guid request)
        {
            List<Category>? category = await _datacontext.Category
                .Where(u => u.UserId == request).ToListAsync();
            return category;
        }
        public async Task<List<Category>> GetAllCategory()
        {
            return await _datacontext.Category.ToListAsync();
        }
        public async Task<Category?> GetCategoryById(Guid request)
        {
            Category? category = await _datacontext.Category.FindAsync(request);
            if (category == null)
            {
                return null;
            }
            return category;
        }
        public async Task<Category?> AddCategory(CategoryInputDto request, Guid sender)
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
            return newCategory;
        }
        public async Task<Category?> UpdateCategoryById(Guid request, CategoryInputDto newCategory)
        {
            Category? category = await _datacontext.Category.FindAsync(request);
            if (category == null)
            {
                return null;
            }
            category.CategoryName = newCategory.CategoryName;
            category.CategoryDescription = newCategory.CategoryDescription;
            category.UpdatedAt = DateTime.Now;
            await _datacontext.SaveChangesAsync();
            return category;
        }
        public async Task<Category?> DeleteCategoryById(Guid request)
        {
            Category? category = await _datacontext.Category
                .FindAsync(request);
            if (category == null)
            {
                return null;
            }
            _datacontext.Category.Remove(category);
            await _datacontext.SaveChangesAsync();
            return category;
        }
    }
}
