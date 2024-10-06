using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Domain.Repositories
{
    public interface IFriendRequestRepo : IRepository<FriendRequest>
    {
        IQueryable<FriendRequest> GetReceivedFriendRequests(string username);
        IQueryable<FriendRequest> GetSentFriendRequests(string username);
    }
}
