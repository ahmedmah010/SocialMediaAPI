using Microsoft.EntityFrameworkCore;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using SocialMediaAPI.Domain.Repositories;
using SocialMediaAPI.Domain.Repositories.Base;
using SocialMediaAPI.Infrastructure.Context;
using SocialMediaAPI.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Infrastructure.Repositories
{
    public class FriendRequestRepo : Repository<FriendRequest>, IFriendRequestRepo
    {
        public FriendRequestRepo(AppDbContext context) : base(context) 
        {

        }
        public IQueryable<FriendRequest> GetReceivedFriendRequests(string username)
        {
           return _context.Set<FriendRequest>().Where(r=>r.Receiver.UserName == username && r.Status == FriendRequestStatus.Pending);
        }
        public IQueryable<FriendRequest> GetSentFriendRequests(string username)
        {
            return _context.Set<FriendRequest>().Where(r => r.Requester.UserName == username && r.Status == FriendRequestStatus.Pending);
        }
    }
}
