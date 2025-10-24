namespace efcore.Models
{
    public class SkillOutputDto
    {
        public Guid SkillId { get; set; }
        public string SkillName { get; set; } = string.Empty;
        public string SkillDescription { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
