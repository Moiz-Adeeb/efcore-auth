namespace efcore.Models
{
    public class ActivityOutputDto
    {
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public ICollection<SkillOutputDto> Skills { get; set; }
    }
}
