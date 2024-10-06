using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Repositories
{
    public interface IFriendShipRepo : IRepository<FriendShip>
    {
        IQueryable<AppUser> GetAllFriends(int userId);
        Task<FriendShip?> GetFriendShipAsync(int currentUserId, int targetFriendId);

    }
}
