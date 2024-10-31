using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Repositories
{
    public interface IStoryRepo : IRepository<Story>
    {
        Task<Story?> GetStoryWithUserAndViewersWithUsers(int storyId);
    }
}
