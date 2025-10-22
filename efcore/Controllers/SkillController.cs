using efcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _SkillService;

        public SkillController(ISkillService SkillService)
        {
            this._SkillService = SkillService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<SkillOutputDto>?>> GetSkill()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<SkillOutputDto>? skills = await _SkillService.GetSkill(request);
                if (skills == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(skills);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<SkillOutputDto>>> GetAllSkill()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid request))
            {
                List<SkillOutputDto> skills = await _SkillService.GetAllSkill();
                if (skills == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(skills);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<SkillOutputDto>> GetSkillById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                SkillOutputDto? skills = await _SkillService.GetSkillById(request, sender);
                if (skills == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(skills);
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string?>> AddSkill(SkillInputDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var sender = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid sender))
            {
                return Ok(await _SkillService.AddSkill(request, sender));
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpPut("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<string>> UpdateSkillById(Guid request, SkillInputDto newSkill)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var skills = await _SkillService.UpdateSkillById(request, newSkill, sender);
                if (skills == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(skills);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<string>>> DeleteSkillById(Guid request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid sender))
            {
                var skills = await _SkillService.DeleteSkillById(request, sender);
                if (skills == null)
                {
                    return NotFound("Skill not Found");
                }
                return Ok(skills);
            }
            else
            {
                return BadRequest("Invalid user ID.");
            }
        }
    }
}
