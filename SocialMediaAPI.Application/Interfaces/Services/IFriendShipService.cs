using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IFriendShipService
    {
        Task<ResponseDTO> SendFriendRequestAsync(string username);
        //Task<ResponseDTO> AddFriendAsync(string username);
        Task<ResponseDTO> RemoveFriendAsync(int userId);
        Task<ResponseDTO> AcceptFriendRequestAsync(int requestId);
        Task<ResponseDTO> RejectFriendRequestAsync(int requestId);
        Task<ResponseDTO> GetReceivedFriendRequestsAsync(string username);
        Task<ResponseDTO> GetSentFriendRequestsAsync(string username);
        Task<ResponseDTO> GetAllFriendsAsync(string username);
    }
}
