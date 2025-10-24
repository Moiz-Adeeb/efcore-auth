using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace efcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            this._skillService = skillService;
        }
        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<List<SkillOutputDto>?> GetSkill()
        {
            return await _skillService.GetSkill();

        }
        [HttpGet("getall")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<SkillOutputDto>?> GetAllSkill()
        {
            return await _skillService.GetAllSkill();
        }
        [HttpGet("getskill")]
        [Authorize(Roles = "User")]
        public async Task<SkillOutputDto?> GetSkillById(Guid request)
        {
            return await _skillService.GetSkillById(request);
        }
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<string?> AddSkill(SkillInputDto request)
        {
            return await _skillService.AddSkill(request);
        }
        [HttpPut($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User")]
        public async Task<string?> UpdateSkillById(Guid request, SkillInputDto newSkill)
        {
            return await _skillService.UpdateSkillById(request, newSkill);
        }
        [HttpDelete($"{{{nameof(request)}}}")]
        [Authorize(Roles = "User, Admin, Manager")]
        public async Task<string?> DeleteSkillById(Guid request)
        {
            return await _skillService.DeleteSkillById(request);
        }
    }
}
