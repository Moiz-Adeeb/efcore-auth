namespace efcore.Models
{
    public class ActivityInputDto
    {
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public List<Guid> SkillIds { get; set; } = new List<Guid>();
    }
}
