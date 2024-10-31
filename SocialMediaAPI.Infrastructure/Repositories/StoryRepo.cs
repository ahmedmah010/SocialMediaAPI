using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories;
using SocialMediaAPI.Infrastructure.Context;
using SocialMediaAPI.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.Repositories
{
    public class StoryRepo : Repository<Story>, IStoryRepo
    {
        public StoryRepo(AppDbContext context) : base(context) { }

        public async Task<Story?> GetStoryWithUserAndViewersWithUsers(int storyId)
        {
            return await _context.Set<Story>()
                .Where(s => s.Id == storyId)
                .Include(s => s.User)
                .Include(s => s.Viewers)
                .ThenInclude(sv => sv.Viewer)
                .FirstOrDefaultAsync();
        }
    }
}
