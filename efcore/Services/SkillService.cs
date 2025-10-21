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
        public async Task<List<Skill>?> GetSkill(Guid request)
        {
            List<Skill>? skill = await _datacontext.Skill
                .Where(u => u.UserId == request).ToListAsync();
            return skill;
        }
        public async Task<List<Skill>> GetAllSkill()
        {
            return await _datacontext.Skill.ToListAsync();
        }
        public async Task<Skill?> GetSkillById(Guid request)
        {
            Skill? skill = await _datacontext.Skill.FindAsync(request);
            if (skill == null)
            {
                return null;
            }
            return skill;
        }
        public async Task<Skill?> AddSkill(SkillInputDto request, Guid sender)
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
            return newSkill;
        }
        public async Task<Skill?> UpdateSkillById(Guid request, SkillInputDto newSkill)
        {
            Skill? skill = await _datacontext.Skill.FindAsync(request);
            if (skill == null)
            {
                return null;
            }
            skill.SkillName = newSkill.SkillName;
            skill.SkillDescription = newSkill.SkillDescription;
            skill.UpdatedAt = DateTime.Now;
            await _datacontext.SaveChangesAsync();
            return skill;
        }
        public async Task<Skill?> DeleteSkillById(Guid request)
        {
            Skill? skill = await _datacontext.Skill
                .FindAsync(request);
            if (skill == null)
            {
                return null;
            }
            _datacontext.Skill.Remove(skill);
            await _datacontext.SaveChangesAsync();
            return skill;
        }
    }
}
