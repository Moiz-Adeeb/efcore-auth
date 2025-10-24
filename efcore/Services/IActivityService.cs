namespace efcore.Services
{
    public interface IActivityService
    {
        Task<List<ActivityOutputDto>?> GetActivity();
        Task<List<ActivityOutputDto>?> GetAllActivity();
        Task<ActivityOutputDto?> GetActivityById(Guid request);
        Task<string?> AddActivity(ActivityInputDto request);
        Task<string?> UpdateActivityById(Guid request, ActivityInputDto newActivity);
        Task<string?> DeleteActivityById(Guid request);
    }
}
