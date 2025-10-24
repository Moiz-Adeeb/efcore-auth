namespace efcore.Services
{
    public class SkillService : ISkillService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public SkillService(DataContext dataContext, IHttpContextAccessor contextAccessor)
        {
            _dataContext = dataContext;
            _contextAccessor = contextAccessor;
        }
        public async Task<List<SkillOutputDto>?> GetSkill()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var skill = await _dataContext.Skill
                .Where(u => u.UserId == id)
                .AsNoTracking()
                .Select(c => new SkillOutputDto
                {
                    SkillId = c.SkillId,
                    SkillName = c.SkillName,
                    SkillDescription = c.SkillDescription,
                    UserName = c.User.Username

                })
                .ToListAsync();
            return skill;
        }
        public async Task<List<SkillOutputDto>?> GetAllSkill()
        {
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (role is not ("Admin" or "Manager")) return null;
            return await _dataContext.Skill
                .AsNoTracking()
                .Select(c => new SkillOutputDto
                {
                    SkillId = c.SkillId,
                    SkillName = c.SkillName,
                    SkillDescription = c.SkillDescription,
                    UserName = c.User.Username

                })
                .ToListAsync();
        }
        public async Task<SkillOutputDto?> GetSkillById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var skill = await _dataContext.Skill.FindAsync(request);
            if (skill == null) return null;
            return skill.UserId != id ? null : skill.ToOutputDto();
        }
        public async Task<string?> AddSkill(SkillInputDto request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var newSkill = new Skill
            {
                SkillId = Guid.NewGuid(),
                SkillName = request.SkillName,
                SkillDescription = request.SkillDescription,
                UserId = id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _dataContext.Skill.AddAsync(newSkill);
            await _dataContext.SaveChangesAsync();
            return "Skill Added Successfully";
        }
        public async Task<string?> UpdateSkillById(Guid request, SkillInputDto newSkill)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var skill = await _dataContext.Skill.FindAsync(request);
            if (skill == null) return "Skill not Found";
            if (skill.UserId != id) return "Unauthorized to update this category";
            skill.SkillName = newSkill.SkillName;
            skill.SkillDescription = newSkill.SkillDescription;
            skill.UpdatedAt = DateTime.Now;
            await _dataContext.SaveChangesAsync();
            return "Skill Updated Successfully";
        }
        public async Task<string?> DeleteSkillById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var skill = await _dataContext.Skill
                .FindAsync(request);
            if (skill == null) return "Skill not Found";
            if (skill.UserId == id || role == "Manager" || role == "Admin")
            {
                _dataContext.Skill.Remove(skill);
                await _dataContext.SaveChangesAsync();
                return "Skill Deleted Successfully";
            } else return "Unauthorized to delete this skill";
        }
    }
}
