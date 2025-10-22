using efcore.Data;
using Org.BouncyCastle.Asn1.Ocsp;

namespace efcore.Services
{
    public class SkillService : ISkillService
    {
        private readonly DataContext _datacontext;
        public SkillService(DataContext datacontext)
        {
            _datacontext = datacontext;
        }
        public async Task<List<SkillOutputDto>?> GetSkill(Guid request)
        {
            var skill = await _datacontext.Skill
                .Where(u => u.UserId == request)
                .Select(c => c.ToOutputDto())
                .ToListAsync();
            return skill;
        }
        public async Task<List<SkillOutputDto>> GetAllSkill()
        {
            return await _datacontext.Skill.Select(c => c.ToOutputDto()).ToListAsync();
        }
        public async Task<SkillOutputDto?> GetSkillById(Guid request, Guid sender)
        {
            Skill? skill = await _datacontext.Skill.FindAsync(request);
            if (skill == null)
            {
                return null;
            }
            if (skill.UserId != sender)
            {
                return null;
            }
            return skill.ToOutputDto();
        }
        public async Task<string?> AddSkill(SkillInputDto request, Guid sender)
        {
            var newSkill = new Skill
            {
                SkillId = Guid.NewGuid(),
                SkillName = request.SkillName,
                SkillDescription = request.SkillDescription,
                UserId = sender,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _datacontext.Skill.AddAsync(newSkill);
            _datacontext.SaveChanges();
            return "Skill Added Successfully";
        }
        public async Task<string?> UpdateSkillById(Guid request, SkillInputDto newSkill, Guid sender)
        {
            Skill? skill = await _datacontext.Skill.FindAsync(request);
            if (skill == null)
            {
                return "Skill not Found";
            }
            if (skill.UserId != sender)
            {
                return "Unauthorized to update this category";
            }
            skill.SkillName = newSkill.SkillName;
            skill.SkillDescription = newSkill.SkillDescription;
            skill.UpdatedAt = DateTime.Now;
            await _datacontext.SaveChangesAsync();
            return "Skill Updated Successfully";
        }
        public async Task<string?> DeleteSkillById(Guid request, Guid sender)
        {
            Skill? skill = await _datacontext.Skill
                .FindAsync(request);
            if (skill == null)
            {
                return "Skill not Found";
            }
            if (skill.UserId != sender)
            {
                return "Unauthorized to delete this skill";
            }
            _datacontext.Skill.Remove(skill);
            await _datacontext.SaveChangesAsync();
            return "Skill Deletted Successfully";
        }
    }
}
