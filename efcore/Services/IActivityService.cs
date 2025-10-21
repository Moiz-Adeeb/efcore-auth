namespace efcore.Services
{
    public interface IActivityService
    {
        Task<List<Activity>?> GetActivity(Guid request);
        Task<List<Activity>> GetAllActivity();
        Task<ActivityOutputDto?> GetActivityById(Guid request);
        Task<Activity?> AddActivity(ActivityInputDto request, Guid sender);
        Task<Activity?> UpdateActivityById(Guid request, ActivityInputDto newActivity);
        Task<Activity?> DeleteActivityById(Guid request);
    }
}
