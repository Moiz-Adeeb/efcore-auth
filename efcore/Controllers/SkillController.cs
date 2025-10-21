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
        public async Task<ActionResult<List<Skill>?>> GetSkill()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var request = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid request))
            {
                return Ok(await _SkillService.GetSkill(request));
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Skill>>> GetAllSkill()
        {
            return Ok(await _SkillService.GetAllSkill());
        }
        [HttpGet("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Skill>> GetSkillById(Guid request)
        {
            Skill? skill = await _SkillService.GetSkillById(request);
            if (skill == null)
            {
                return NotFound("Skill not Found");
            }
            return Ok(skill);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Skill?>> AddSkill(SkillInputDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var sender = Guid.Parse(userId);
            if (Guid.TryParse(userId, out Guid sender))
            {
                return Ok(await _SkillService.AddSkill(request, sender));
            }
            else
            {
                return BadRequest("Invalid user ID");
            }
        }
        [HttpPut("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<Skill>> UpdateSkillById(Guid request, SkillInputDto newSkill)
        {
            var skill = await _SkillService.UpdateSkillById(request, newSkill);
            if (skill == null)
            {
                return NotFound("Skill not Found");
            }
            return Ok(skill);
        }
        [HttpDelete("{request}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<List<Skill>>> DeleteSkillById(Guid request)
        {
            var skill = await _SkillService.DeleteSkillById(request);
            if (skill == null)
            {
                return NotFound("Skill not Found");
            }
            return Ok(skill);
        }
    }
}
