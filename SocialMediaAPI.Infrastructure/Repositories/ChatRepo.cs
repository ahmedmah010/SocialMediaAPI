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
    public class ChatRepo : Repository<Chat>, IChatRepo
    {
        public ChatRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Chat>> GetChatsWithLastMessageAsync(int userId, int pageNumber, int pageSize)
        {
            int skipCount = (pageNumber - 1) * pageSize;
            return await _context.Set<Chat>().Where(c => c.Participant1Id == userId || c.Participant2Id == userId)
                .Skip(skipCount).Take(pageSize)
                .Select(c => new Chat
                {
                    Id = c.Id,
                    Participant1Id = c.Participant1Id,
                    Participant2Id = c.Participant2Id,
                    Messages = c.Messages.OrderByDescending(m => m.SentAt).Take(1).ToList()
                })
                .ToListAsync();
        }
    }
}
