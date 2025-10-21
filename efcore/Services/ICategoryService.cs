namespace efcore.Services
{
    public interface ICategoryService
    {
        Task<List<Category>?> GetCategory(Guid request);
        Task<List<Category>> GetAllCategory();
        Task<Category?> GetCategoryById(Guid request);
        Task<Category?> AddCategory(CategoryInputDto request, Guid sender);
        Task<Category?> UpdateCategoryById(Guid request, CategoryInputDto newCategory);
        Task<Category?> DeleteCategoryById(Guid request);
    }
}
