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

        public async Task<List<ActivityOutputDto>> GetActivity(Guid request)
        {
            List<ActivityOutputDto> activities = await _datacontext.Activity
                    .AsNoTracking()
                    .Where(a => a.UserId == request)
                    .Select(a => new ActivityOutputDto
                    {
                        ActivityId = a.ActivityId,
                        ActivityName = a.ActivityName,
                        ActivityDescription = a.ActivityDescription,
                        UserName = a.User.Username,
                        CategoryId = a.CategoryId,
                        CategoryName = a.Category.CategoryName,
                        CategoryDescription = a.Category.CategoryDescription,
                        Skills = a.ActivitySkills.Select(s => new SkillOutputDto
                        {
                            SkillId = s.Skill.SkillId,
                            SkillName = s.Skill.SkillName,
                            SkillDescription = s.Skill.SkillDescription
                        }).ToList()
                    })
                    .ToListAsync();

            return activities;
        }
        public async Task<List<ActivityOutputDto>> GetAllActivity()
        {
            return await _datacontext.Activity.Select(c => c.ToOutputDto()).ToListAsync();
        }
        public async Task<ActivityOutputDto?> GetActivityById(Guid request, Guid sender)
        {
            var activity = await _datacontext.Activity
                .Where(a => a.ActivityId == request && a.UserId == sender)
                .Select(a => new ActivityOutputDto
                {
                    ActivityId = a.ActivityId,
                    ActivityName = a.ActivityName,
                    ActivityDescription = a.ActivityDescription,
                    UserName = a.User.Username,
                    CategoryId = a.CategoryId,
                    CategoryName = a.Category.CategoryName,
                    CategoryDescription = a.Category.CategoryDescription,
                    Skills = a.ActivitySkills.Select(s => new SkillOutputDto
                    {
                        SkillId = s.Skill.SkillId,
                        SkillName = s.Skill.SkillName,
                        SkillDescription = s.Skill.SkillDescription
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (activity == null)
            {
                return null;
            }

            return activity;
        }
        public async Task<string?> AddActivity(ActivityInputDto request, Guid sender)
        {
            List<Skill> skills = await _datacontext.Skill
                            .Where(s => request.SkillIds.Contains(s.SkillId))
                            .ToListAsync();
            Activity newActivity = new()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = request.ActivityName,
                ActivityDescription = request.ActivityDescription,
                UserId = sender,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            foreach (var skillId in skills)
            {
                newActivity.ActivitySkills.Add(new ActivitySkill { Skill = skillId });
            }
            await _datacontext.Activity.AddAsync(newActivity);
            await _datacontext.SaveChangesAsync();
            return "Activity Added Successfully";
        }
        public async Task<string?> UpdateActivityById(Guid request, ActivityInputDto newActivity, Guid sender)
        {
            Activity? Activity = await _datacontext.Activity
                .Include(a => a.ActivitySkills)
                .FirstOrDefaultAsync(a => a.ActivityId == request);
            if (Activity == null)
            {
                return null;
            }
            if (Activity.UserId != sender)
            {
                return "Unauthorized to update this Activity";
            }
            Activity.ActivityName = newActivity.ActivityName;
            Activity.ActivityDescription = newActivity.ActivityDescription;
            Activity.CategoryId = newActivity.CategoryId;
            Activity.UpdatedAt = DateTime.UtcNow;

            HashSet<Guid> requestedSkillIds = newActivity.SkillIds.ToHashSet();
            HashSet<Guid> currentSkillIds = Activity.ActivitySkills.Select(s => s.SkillId).ToHashSet();

            List<ActivitySkill>? skillsToRemove = Activity.ActivitySkills
                .Where(s => !requestedSkillIds.Contains(s.SkillId))
                .ToList();

            List<Guid>? skillIdsToAdd = requestedSkillIds
                .Except(currentSkillIds)
                .ToList();

            if (skillsToRemove.Count > 0)
                _datacontext.Activity_Skills.RemoveRange(skillsToRemove);

            if (skillIdsToAdd.Count > 0)
            {
                var newSkills = skillIdsToAdd.Select(skillId => new ActivitySkill
                {
                    ActivityId = Activity.ActivityId,
                    SkillId = skillId
                });

                await _datacontext.Activity_Skills.AddRangeAsync(newSkills);
            }
            await _datacontext.SaveChangesAsync();
            return "Activity Updated Successfully";
        }
        public async Task<string?> DeleteActivityById(Guid request, Guid sender)
        {
            Activity? Activity = await _datacontext.Activity
                .FindAsync(request);
            if (Activity == null)
            {
                return null;
            }
            if (Activity.UserId != sender)
            {
                return "Unauthorized to delete this Activity";
            }

            List<ActivitySkill> skills = await _datacontext.Activity_Skills
                .Where(a => a.ActivityId == request).ToListAsync();
            _datacontext.Activity.Remove(Activity);
            _datacontext.Activity_Skills.RemoveRange(skills);
            await _datacontext.SaveChangesAsync();
            return "Activity Deleted Successfully";
        }
    }
}
