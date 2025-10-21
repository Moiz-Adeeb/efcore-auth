using efcore.Data;
using Microsoft.AspNetCore.Components.Forms;

namespace efcore.Services
{
    public class ActivityService : IActivityService
    {
        private readonly DataContext _datacontext;

        public ActivityService(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        public async Task<List<Activity>?> GetActivity(Guid request)
        {
            List<Activity>? Activity = await _datacontext.Activity
                .Where(u => u.UserId == request).ToListAsync();
            return Activity;
        }
        public async Task<List<Activity>> GetAllActivity()
        {
            return await _datacontext.Activity.ToListAsync();
        }
        public async Task<ActivityOutputDto?> GetActivityById(Guid request)
        {
            Activity? activity = await _datacontext.Activity
                        .Include(a => a.ActivitySkills)
                        .ThenInclude(pas => pas.Skill)
                        .FirstOrDefaultAsync(a => a.ActivityId == request);
            if (activity == null)
            {
                return null;
            }
            var ActivityOutputDto = new ActivityOutputDto
            {
                ActivityName = activity.ActivityName,
                // ... other properties
                Skills = activity.ActivitySkills.Select(pas => new SkillOutputDto
                {
                    SkillId = pas.Skill.SkillId,
                    SkillName = pas.Skill.SkillName
                }).ToList()
            };

            return ActivityOutputDto;
        }
        public async Task<Activity?> AddActivity(ActivityInputDto request, Guid sender)
        {
            var skills = await _datacontext.Skill
                            .Where(s => request.SkillIds.Contains(s.SkillId))
                            .ToListAsync();
            var newActivity = new Activity
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = request.ActivityName,
                ActivityDescription = request.ActivityDescription,
                UserId = sender,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            foreach (var skillId in skills)
            {
                newActivity.ActivitySkills.Add(new ActivitySkill { Skill = skillId });
            }
            await _datacontext.Activity.AddAsync(newActivity);
            await _datacontext.SaveChangesAsync();
            return newActivity;
        }
        public async Task<Activity?> UpdateActivityById(Guid request, ActivityInputDto newActivity)
        {
            Activity? Activity = await _datacontext.Activity.FindAsync(request);
            if (Activity == null)
            {
                return null;
            }
            Activity.ActivityName = newActivity.ActivityName;
            Activity.ActivityDescription = newActivity.ActivityDescription;
            Activity.UpdatedAt = DateTime.Now;
            await _datacontext.SaveChangesAsync();
            return Activity;
        }
        public async Task<Activity?> DeleteActivityById(Guid request)
        {
            Activity? Activity = await _datacontext.Activity
                .FindAsync(request);
            if (Activity == null)
            {
                return null;
            }
            _datacontext.Activity.Remove(Activity);
            await _datacontext.SaveChangesAsync();
            return Activity;
        }
    }
}
