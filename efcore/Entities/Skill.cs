namespace efcore.Entities
{
    public class Skill
    {
        public required Guid SkillId { get; set; }
        public required string SkillName { get; set; } = string.Empty;
        public required string SkillDescription { get; set; } = string.Empty;
        public required Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ActivitySkill> ActivitySkills { get; set; } = new List<ActivitySkill>();
    }
}
