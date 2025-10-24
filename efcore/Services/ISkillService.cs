namespace efcore.Services
{
    public interface ISkillService
    {
        Task<List<SkillOutputDto>?> GetSkill();
        Task<List<SkillOutputDto>?> GetAllSkill();
        Task<SkillOutputDto?> GetSkillById(Guid request);
        Task<string?> AddSkill(SkillInputDto request);
        Task<string?> UpdateSkillById(Guid request, SkillInputDto newSkill);
        Task<string?> DeleteSkillById(Guid request);
    }
}
