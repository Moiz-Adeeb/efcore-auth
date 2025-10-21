namespace efcore.Services
{
    public interface ISkillService
    {
        Task<List<Skill>?> GetSkill(Guid request);
        Task<List<Skill>> GetAllSkill();
        Task<Skill?> GetSkillById(Guid request);
        Task<Skill?> AddSkill(SkillInputDto request, Guid sender);
        Task<Skill?> UpdateSkillById(Guid request, SkillInputDto newSkill);
        Task<Skill?> DeleteSkillById(Guid request);
    }
}
