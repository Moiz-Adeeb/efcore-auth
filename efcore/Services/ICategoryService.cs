namespace efcore.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryOutputDto>?> GetCategory();
        Task<List<CategoryOutputDto>?> GetAllCategory();
        Task<CategoryOutputDto?> GetCategoryById(Guid request);
        Task<string?> AddCategory(CategoryInputDto request);
        Task<string?> UpdateCategoryById(Guid request, CategoryInputDto newCategory);
        Task<string?> DeleteCategoryById(Guid request);
    }
}
