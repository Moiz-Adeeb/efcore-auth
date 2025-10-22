namespace efcore.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryOutputDto>?> GetCategory(Guid request);
        Task<List<CategoryOutputDto>> GetAllCategory();
        Task<CategoryOutputDto?> GetCategoryById(Guid request, Guid sender);
        Task<string?> AddCategory(CategoryInputDto request, Guid sender);
        Task<string?> UpdateCategoryById(Guid request, CategoryInputDto newCategory, Guid sender);
        Task<string?> DeleteCategoryById(Guid request, Guid sender);
    }
}
