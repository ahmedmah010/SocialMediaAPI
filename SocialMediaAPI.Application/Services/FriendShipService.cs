using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Enums;
using SocialMediaAPI.Domain.Repositories;
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
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<AppUser> _userManager; 
        private readonly IRepository<FriendRequest> _friendRequestsRepo;
        private readonly IRepository<AppUser> _appUserRepo;
        private readonly IFriendRequestRepo _friendRequestSpecificRepo;
        private readonly IRepository<FriendShip> _friendShipRepo;
        private readonly IResponseService _responseService;
        private readonly IFriendShipRepo _friendShipSpecificRepo;
        private readonly IRepository<FriendRequest> _friendRequestRepo;
        public FriendShipService(ICurrentUserService currentUserService,
            UserManager<AppUser> userManager, 
            IRepository<FriendRequest> friendRequestsRepo,
            IResponseService responseService,
            IRepository<AppUser> appUserRepo,
            IRepository<FriendShip> friendShipRepo,
            IMapper mapper,
            IFriendRequestRepo friendRequestSpecificRepo,
            IFriendShipRepo friendShipSpecificRepo,
            IRepository<FriendRequest> friendRequestRepo)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _friendRequestsRepo = friendRequestsRepo;
            _responseService = responseService;
            _appUserRepo = appUserRepo;
            _friendShipRepo = friendShipRepo;
            _mapper = mapper;
            _friendRequestSpecificRepo = friendRequestSpecificRepo;
            _friendShipSpecificRepo = friendShipSpecificRepo;
            _friendRequestRepo = friendRequestRepo;
        }
        private async Task<ResponseDTO> AddFriendAsync(int id) // Private method, won't be directly used.
        {
            AppUser newFriend = await _userManager.FindByIdAsync(id.ToString());
            if (newFriend != null)
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
        public async Task<ResponseDTO> AcceptFriendRequestAsync(int requestId)
        {
            AppUser currentUser = await _currentUserService.GetCurrentUserAsync();
            FriendRequest? friendRequest = await _friendRequestRepo.FirstOrDefaultAsync(r=>r.Id == requestId && r.ReceiverId == currentUser.Id);
            if (friendRequest != null)
            {
                ResponseDTO result = await AddFriendAsync(friendRequest.RequesterId);
                if (result.Success)
                {
                    friendRequest.Status = FriendRequestStatus.Accepted;
                    await _friendRequestsRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Friend request accepted");
                }
                else
                {
                    return result;
                }
            }
            return await _responseService.GenerateErrorResponseAsync("Friend request not found");
        }

        public async Task<ResponseDTO> RejectFriendRequestAsync(int requestId)
        {
            AppUser currentUser = await _currentUserService.GetCurrentUserAsync();
            FriendRequest? friendRequest = await _friendRequestRepo.FirstOrDefaultAsync(r => r.Id == requestId && r.ReceiverId == currentUser.Id);
            if (friendRequest != null)
            {
                friendRequest.Status = FriendRequestStatus.Rejected;
                await _friendRequestsRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend request rejected");
            }
            return await _responseService.GenerateErrorResponseAsync("Friend request not found");
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

        public async Task<ResponseDTO> GetReceivedFriendRequestsAsync(string username) 
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                List<ReceivedFriendRequestDTO> receivedFriendRequestsDTO = _mapper.ProjectTo<ReceivedFriendRequestDTO>(_friendRequestSpecificRepo.GetReceivedFriendRequests(username)).ToList();
                return await _responseService.GenerateSuccessResponseAsync(data: receivedFriendRequestsDTO);
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }

        public async Task<ResponseDTO> GetSentFriendRequestsAsync(string username)
        {
            AppUser user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                List<SentFriendRequestDTO> sentFriendRequestsDTO = _mapper.ProjectTo<SentFriendRequestDTO>(_friendRequestSpecificRepo.GetSentFriendRequests(username)).ToList();
                return await _responseService.GenerateSuccessResponseAsync(data: sentFriendRequestsDTO);
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }
        public async Task<ResponseDTO> GetAllFriendsAsync(string username)
        {
            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                List<FriendDTO> friends = _mapper.ProjectTo<FriendDTO>(_friendShipSpecificRepo.GetAllFriends(user.Id)).ToList();
                return await _responseService.GenerateSuccessResponseAsync(data: friends);
            }
            return await _responseService.GenerateErrorResponseAsync("User not found");
        }
        public async Task<ResponseDTO> RemoveFriendAsync(int userId)
        {
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            FriendShip? friendShip = await _friendShipSpecificRepo.GetFriendShipAsync(currentUserId, userId);
            if (friendShip != null)
            {
                _friendShipRepo.Remove(friendShip);
                await _friendShipRepo.SaveChangesAsync();
                return await _responseService.GenerateSuccessResponseAsync("Friend removed");
            }
            return await _responseService.GenerateErrorResponseAsync("This friend doesn't exist in your friendlist");
        }

    }
}
