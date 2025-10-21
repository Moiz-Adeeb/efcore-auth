namespace efcore.Entities
{
    public class ActivitySkill
    {
        public Guid ActivityId { get; set; } 
        public Guid SkillId { get; set; } 
        public Activity Activity { get; set; }
        public Skill Skill { get; set; }
    }
}
