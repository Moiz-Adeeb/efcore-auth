using efcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _ActivityService;

        public ActivityController(IActivityService ActivityService)
        {
            this._ActivityService = ActivityService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<Activity>?>> GetActivity()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var request = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid request))
            {
                return Ok(await _ActivityService.GetActivity(request));
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Activity>>> GetAllActivity()
        {
            return Ok(await _ActivityService.GetAllActivity());
        }
        [HttpGet("getActivity")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ActivityOutputDto>> GetActivityById(Guid request)
        {
            ActivityOutputDto? Activity = await _ActivityService.GetActivityById(request);
            if (Activity == null)
            {
                return NotFound("Activity not Found");
            }
            return Ok(Activity);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Activity?>> AddActivity(ActivityInputDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var sender = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid sender))
            {
                return Ok(await _ActivityService.AddActivity(request, sender));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpPut("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Activity>> UpdateActivityById(Guid request, ActivityInputDto newActivity)
        {
            var categories = await _ActivityService.UpdateActivityById(request, newActivity);
            if (categories == null)
            {
                return NotFound("Activity not Found");
            }
            return Ok(categories);
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Activity>> DeleteActivityById(Guid request)
        {
            var Activity = await _ActivityService.DeleteActivityById(request);
            if (Activity == null)
            {
                return NotFound("Activity not Found");
            }
            return Ok(Activity);
        }
    }
}
