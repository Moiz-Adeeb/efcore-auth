namespace efcore.Services
{
    public interface ISkillService
    {
        Task<List<SkillOutputDto>?> GetSkill(Guid request);
        Task<List<SkillOutputDto>> GetAllSkill();
        Task<SkillOutputDto?> GetSkillById(Guid request, Guid sender);
        Task<string?> AddSkill(SkillInputDto request, Guid sender);
        Task<string?> UpdateSkillById(Guid request, SkillInputDto newSkill, Guid sender);
        Task<string?> DeleteSkillById(Guid request, Guid sender);
    }
}
