namespace efcore.Models
{
    public class SkillOutputDto
    {
        public required Guid SkillId { get; set; }
        public required string SkillName { get; set; }
        public required string SkillDescription { get; set; }
    }
}
