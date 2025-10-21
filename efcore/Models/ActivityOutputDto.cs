namespace efcore.Models
{
    public class ActivityOutputDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public ICollection<SkillOutputDto> Skills { get; set; }
    }
}
