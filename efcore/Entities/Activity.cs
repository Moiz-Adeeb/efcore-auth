namespace efcore.Entities
{
    public class Activity
    {
        public Guid ActivityId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public string ActivityDescription { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ActivitySkill> ActivitySkills { get; set; } = new List<ActivitySkill>();
        public User User { get; set; }
        public Category Category { get; set; }
    }
}
