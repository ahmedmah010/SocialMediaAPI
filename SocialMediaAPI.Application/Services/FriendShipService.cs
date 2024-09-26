using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class FriendShipService : IFriendShipService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager; 
        private readonly IRepository<FriendRequest> _friendRequestsRepo;
        private readonly IRepository<AppUser> _appUserRepo;
        private readonly IRepository<FriendShip> _friendShipRepo;
        private readonly IResponseService _responseService;
        public FriendShipService(ICurrentUserService currentUserService,
            UserManager<AppUser> userManager, 
            IRepository<FriendRequest> friendRequestsRepo,
            IResponseService responseService,
            IRepository<AppUser> appUserRepo,
            IRepository<FriendShip> friendShipRepo)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _friendRequestsRepo = friendRequestsRepo;
            _responseService = responseService;
            _appUserRepo = appUserRepo;
            _friendShipRepo = friendShipRepo;
        }
        public async Task<ResponseDTO> AcceptFriendRequestAsync(int requestId)
        {
            AppUser currentUser = await _currentUserService.GetCurrentUserAsync(user => user.ReceivedFriendRequests);
            FriendRequest friendRequest = currentUser.ReceivedFriendRequests.Where(friendRequest => friendRequest.Id == requestId).FirstOrDefault();
            if (friendRequest != null)
            {
                friendRequest.Status = FriendRequestStatus.Accepted;
                await AddFriendAsync(friendRequest.Requester.UserName);
                await _friendRequestsRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend request accepted");
            }
            return await _responseService.GenerateErrorResponseAsync("Friend request not found");
        }

        public async Task<ResponseDTO> AddFriendAsync(string username)
        {
            AppUser newFriend = await _userManager.FindByNameAsync(username); 
            if(newFriend!=null)
            {
                FriendShip newFriendShip = new FriendShip
                {
                    User = await _currentUserService.GetCurrentUserAsync(),
                    Friend = newFriend,
                };
                await _friendShipRepo.AddAsync(newFriendShip);
                await _friendShipRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Added friend successfully");
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }

        public async Task<ResponseDTO> GetReceivedFriendRequestsAsync(string username)
        {
            AppUser user = await _appUserRepo.FindWithIncludesAsync(user => user.UserName == username, user => user.ReceivedFriendRequests);
            if (user != null)
            {
                return await _responseService.GenerateSuccessResponseAsync(data: user.ReceivedFriendRequests);
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }

        public async Task<ResponseDTO> GetSentFriendRequestsAsync(string username)
        {
            AppUser user = await _appUserRepo.FindWithIncludesAsync(user => user.UserName == username, user => user.SentFriendRequests);
            if (user != null)
            {
                return await _responseService.GenerateSuccessResponseAsync(data:user.SentFriendRequests);
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }

        public async Task<ResponseDTO> RejectFriendRequestAsync(int requestId)
        {
            AppUser currentUser = await _currentUserService.GetCurrentUserAsync(user => user.ReceivedFriendRequests);
            FriendRequest friendRequest = currentUser.ReceivedFriendRequests.Where(friendRequest => friendRequest.Id == requestId).FirstOrDefault();
            if (friendRequest != null)
            {
                friendRequest.Status = FriendRequestStatus.Rejected;
                await _friendRequestsRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend request rejected");
            }
            return await _responseService.GenerateErrorResponseAsync("Friend request not found");
        }
        public async Task<ResponseDTO> GetAllFriendsAsync(string username)
        {
            AppUser user = await _appUserRepo.FindWithIncludesAsync(user => user.UserName == username, user => user.Friends);
            if (user != null)
            {
                List<AppUser> friends = user.Friends.Select(friendShip => friendShip.Friend).ToList();
                return await _responseService.GenerateSuccessResponseAsync(data: friends);
            }
            return await _responseService.GenerateErrorResponseAsync("Invalid user");
        }
        public async Task<ResponseDTO> RemoveFriendAsync(string username)
        {
            AppUser currentUser = await _currentUserService.GetCurrentUserAsync(u => u.Friends);
            FriendShip? friendShip = currentUser.Friends?.Where(friendShip=>friendShip.Friend.UserName == username)?.FirstOrDefault();
            if (friendShip != null)
            {
                _friendShipRepo.Remove(friendShip);
                await _friendShipRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend removed");
            }
            return await _responseService.GenerateErrorResponseAsync("This friend doesn't exist in your friendlist");
        }

        public async Task<ResponseDTO> SendFriendRequestAsync(string username)
        {
            AppUser? Requester = await _currentUserService.GetCurrentUserAsync();
            AppUser? Receiver = await _userManager.FindByNameAsync(username);
            if (Receiver != null && Requester != null)
            {
                FriendRequest newFriendRequest = new FriendRequest
                {
                    CreatedAt = DateTime.Now,
                    Receiver = Receiver,
                    Requester = Requester,
                    Status = FriendRequestStatus.Pending,
                };
                await _friendRequestsRepo.AddAsync(newFriendRequest);
                await _friendRequestsRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend request sent");
            } 
            return await _responseService.GenerateErrorResponseAsync("Something wrong happened");
        }
    }
}
