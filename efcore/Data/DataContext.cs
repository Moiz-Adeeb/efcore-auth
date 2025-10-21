
namespace efcore.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Skill> Skill { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<ActivitySkill> Activity_Skills { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the composite primary key for the join entity
            modelBuilder.Entity<ActivitySkill>()
                .HasKey(pas => new { pas.ActivityId, pas.SkillId });

            // Configure the many-to-many relationship
            modelBuilder.Entity<ActivitySkill>()
                .HasOne(pas => pas.Activity)
                .WithMany(a => a.ActivitySkills)
                .HasForeignKey(pas => pas.ActivityId);

            modelBuilder.Entity<ActivitySkill>()
                .HasOne(pas => pas.Skill)
                .WithMany(s => s.ActivitySkills)
                .HasForeignKey(pas => pas.SkillId);
        }
    }
}
