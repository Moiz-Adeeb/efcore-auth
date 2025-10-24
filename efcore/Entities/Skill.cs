namespace efcore.Entities
{
    public class Skill
    {
        public  Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string SkillDescription { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ActivitySkill> ActivitySkills { get; set; } = new List<ActivitySkill>();
        public SkillInputDto ToInputDto()
        {
            return new SkillInputDto
            {
                SkillName = this.SkillName,
                SkillDescription = this.SkillDescription
            };
        }
        public SkillOutputDto ToOutputDto()
        {
            return new SkillOutputDto
            {
                SkillId = this.SkillId,
                SkillName = this.SkillName,
                SkillDescription = this.SkillDescription,
                UserName = this.User.Username
            };
        }
    }
    public static class SkillExtensions
    {
        public static List<SkillOutputDto> ToOutputDtoList(this IEnumerable<Skill> skills)
            => skills.Select(c => c.ToOutputDto()).ToList();
    }
}
