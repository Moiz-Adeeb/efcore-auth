namespace efcore.Services
{
    public class ActivityService : IActivityService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public ActivityService(DataContext dataContext, IHttpContextAccessor contextAccessor)
        {
            _dataContext = dataContext;
            _contextAccessor = contextAccessor;
        }

        public async Task<List<ActivityOutputDto>?> GetActivity()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var id)!) return null;
            var activities = await _dataContext.Activity
                    .AsNoTracking()
                    .Where(a => a.UserId == id)
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
        public async Task<List<ActivityOutputDto>?> GetAllActivity()
        {
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role)!;
            if (role is not ("Admin" or "Manager")) return null;
            var activities = await _dataContext.Activity
                .AsNoTracking()
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
        public async Task<ActivityOutputDto?> GetActivityById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (Guid.TryParse(userId, out var id)!) return null;
            
            var activity = await _dataContext.Activity
                .Where(a => a.ActivityId == request && a.UserId == id)
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
            
            return activity ?? null;
        }
        public async Task<string?> AddActivity(ActivityInputDto request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out var id)) {}
            var skills = await _dataContext.Skill
                            .Where(s => request.SkillIds.Contains(s.SkillId))
                            .ToListAsync();
            Activity newActivity = new()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = request.ActivityName,
                ActivityDescription = request.ActivityDescription,
                UserId = id,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            foreach (var skillId in skills)
            {
                newActivity.ActivitySkills.Add(new ActivitySkill { Skill = skillId });
            }
            await _dataContext.Activity.AddAsync(newActivity);
            await _dataContext.SaveChangesAsync();
            return "Activity Added Successfully";
        }
        public async Task<string?> UpdateActivityById(Guid request, ActivityInputDto newActivity)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var activity = await _dataContext.Activity
                .Include(a => a.ActivitySkills)
                .FirstOrDefaultAsync(a => a.ActivityId == request);
            if (activity == null)
            {
                return null;
            }
            if (activity.UserId.ToString() != userId)
            {
                return "Unauthorized to update this Activity";
            }
            activity.ActivityName = newActivity.ActivityName;
            activity.ActivityDescription = newActivity.ActivityDescription;
            activity.CategoryId = newActivity.CategoryId;
            activity.UpdatedAt = DateTime.UtcNow;

            var requestedSkillIds = newActivity.SkillIds.ToHashSet();
            var currentSkillIds = activity.ActivitySkills.Select(s => s.SkillId).ToHashSet();

            var skillsToRemove = activity.ActivitySkills
                .Where(s => !requestedSkillIds.Contains(s.SkillId))
                .ToList();

            var skillIdsToAdd = requestedSkillIds
                .Except(currentSkillIds)
                .ToList();

            if (skillsToRemove.Count > 0)
                _dataContext.Activity_Skills.RemoveRange(skillsToRemove);

            if (skillIdsToAdd.Count > 0)
            {
                var newSkills = skillIdsToAdd.Select(skillId => new ActivitySkill
                {
                    ActivityId = activity.ActivityId,
                    SkillId = skillId
                });

                await _dataContext.Activity_Skills.AddRangeAsync(newSkills);
            }
            await _dataContext.SaveChangesAsync();
            return "Activity Updated Successfully";
        }
        public async Task<string?> DeleteActivityById(Guid request)
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            var activity = await _dataContext.Activity
                .FindAsync(request);
            if (activity == null)
            {
                return null;
            }
            if (activity.UserId.ToString() == userId || role == "Manager" || role == "Admin")
            {
                var skills = await _dataContext.Activity_Skills
                .Where(a => a.ActivityId == request).ToListAsync();
                _dataContext.Activity.Remove(activity);
                _dataContext.Activity_Skills.RemoveRange(skills);
                await _dataContext.SaveChangesAsync();
                return "Activity Deleted Successfully";
            } else
            {
                return "Unauthorized to delete this Activity";
            }

        }
    }
}
