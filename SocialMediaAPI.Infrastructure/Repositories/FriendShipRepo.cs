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
    public class FriendShipRepo : Repository<FriendShip>, IFriendShipRepo
    {
        public FriendShipRepo(AppDbContext context) : base(context)
        { }
        
        public IQueryable<AppUser> GetAllFriends(int userId)
        {
           return _context.Set<FriendShip>().Where(fs => fs.UserId == userId || fs.FriendId == userId)
                           .Include(fs => fs.User) 
                           .Include(fs => fs.Friend)
                           .Select(fs => fs.UserId == userId ? fs.Friend : fs.User);
        }
        public async Task<FriendShip?> GetFriendShipAsync(int currentUserId, int targetFriendId)
        {
            return await _context.Set<FriendShip>().FirstOrDefaultAsync(fs =>
            (fs.UserId == currentUserId && fs.FriendId == targetFriendId) || 
            (fs.UserId == targetFriendId && fs.FriendId == currentUserId)
            );
                                             
        }
    }
}
