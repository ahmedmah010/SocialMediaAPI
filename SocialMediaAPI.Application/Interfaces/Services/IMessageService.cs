using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IMessageService
    {
        Task<ResponseDTO> MarkMessageAsSeenAsync(List<int> messagesIds);
        Task<ResponseDTO> SendMessageAsync(MessageDTO message, int receiverId);
        Task<ResponseDTO> GetChatAsync(int userId, int pageNumber = 1, int pageSize = 20);
        Task<ResponseDTO> GetChatsAsync(int pageNumber = 1, int pageSize = 5);
    }
}
