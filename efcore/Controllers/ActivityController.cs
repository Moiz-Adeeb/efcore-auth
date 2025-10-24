using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            this._activityService = activityService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<List<ActivityOutputDto>?> GetActivity()
        {
            return await _activityService.GetActivity();
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<ActivityOutputDto>?> GetAllActivity()
        {
            return await _activityService.GetAllActivity();
        }
        [HttpGet("getActivity")]
        [Authorize(Roles = "User")]
        public async Task<ActivityOutputDto?> GetActivityById(Guid request)
        {
            return await _activityService.GetActivityById(request);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<string?> AddActivity(ActivityInputDto request)
        {
                return await _activityService.AddActivity(request);
        }
        [HttpPut($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User")]
        public async Task<string?> UpdateActivityById(Guid request, ActivityInputDto newActivity)
        {
            return await _activityService.UpdateActivityById(request, newActivity);
        }
        [HttpDelete($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User, Admin, Manager")]
        public async Task<string?> DeleteActivityById(Guid request)
        {
            return await _activityService.DeleteActivityById(request);
        }
    }
}
