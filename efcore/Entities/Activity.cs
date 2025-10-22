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
        public ICollection<ActivitySkill> ActivitySkills { get; set; } = [];
        public User User { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ActivityInputDto ToInputDto()
        {
            return new ActivityInputDto
            {
                ActivityName = this.ActivityName,
                ActivityDescription = this.ActivityDescription,
                CategoryId = this.CategoryId
            };
        }
        public ActivityOutputDto ToOutputDto()
        {
            return new ActivityOutputDto
            {
                ActivityId = this.ActivityId,
                ActivityName = this.ActivityName,
                ActivityDescription = this.ActivityDescription,
                UserName = this.User.Username,
                CategoryId = this.CategoryId,
                CategoryName = this.Category.CategoryName,
                CategoryDescription = this.Category.CategoryDescription,
                Skills = this.ActivitySkills.Select(asg => asg.Skill.ToOutputDto()).ToList()
            };
        }
    }
    public static class ActivityExtensions
    {
        public static List<ActivityOutputDto> ToOutputDtoList(this IEnumerable<Activity> activities)
            => activities.Select(c => c.ToOutputDto()).ToList();
    }
}
