namespace efcore.Services
{
    public interface IActivityService
    {
        Task<List<ActivityOutputDto>> GetActivity(Guid request);
        Task<List<ActivityOutputDto>> GetAllActivity();
        Task<ActivityOutputDto?> GetActivityById(Guid request, Guid sender);
        Task<string?> AddActivity(ActivityInputDto request, Guid sender);
        Task<string?> UpdateActivityById(Guid request, ActivityInputDto newActivity, Guid sender);
        Task<string?> DeleteActivityById(Guid request, Guid sender);
    }
}
