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
        public async Task<ActionResult<List<ActivityOutputDto>?>> GetActivity()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<ActivityOutputDto>? activities = await _ActivityService.GetActivity(request);
                if (activities == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(activities);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<ActivityOutputDto>>> GetAllActivity()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<ActivityOutputDto> activities = await _ActivityService.GetAllActivity();
                if (activities == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(activities);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getActivity")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<ActivityOutputDto>> GetActivityById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                ActivityOutputDto? activities = await _ActivityService.GetActivityById(request, sender);
                if (activities == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(activities);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string?>> AddActivity(ActivityInputDto request)
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
        public async Task<ActionResult<string>> UpdateActivityById(Guid request, ActivityInputDto newActivity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var activities = await _ActivityService.UpdateActivityById(request, newActivity, sender);
                if (activities == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(activities);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> DeleteActivityById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var activities = await _ActivityService.DeleteActivityById(request, sender);
                if (activities == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(activities);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
    }
}
