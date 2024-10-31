﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class StoryService : IStoryService
    {
        private readonly IResponseService _responseService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<Story> _storyRepo;
        private readonly IStoryRepo _storySpecificRepo;
        private readonly IRepository<AppUser> _appUserRepo;
        private readonly IRepository<StoryViewer> _storyViewerRepo;
        private readonly IValidator<StoryDTO> _storyDtoValidator;
        private readonly List<string> allowedFilesExtenstions = new List<string> {".jpg", ".jpeg", ".png", ".svg", ".mp4", ".avi"};
        private readonly string _wwwrootPath;
        public StoryService(IResponseService responseService,
            ICurrentUserService currentUserService,
            IRepository<Story> storyRepo, 
            IRepository<AppUser> appUserRepo,
            IValidator<StoryDTO> storyDtoValidator,
            IRepository<StoryViewer> storyViewerRepo,
            IStoryRepo storySpecificRepo)
        {
            _responseService = responseService;
            _currentUserService = currentUserService;
            _storyRepo = storyRepo;
            _appUserRepo = appUserRepo;
            _storyDtoValidator = storyDtoValidator;
            _wwwrootPath = Path.GetFullPath("wwwroot");
            _storyViewerRepo = storyViewerRepo;
            _storySpecificRepo = storySpecificRepo;
        }

        /* Helper Methods */
        private bool CheckFileExtension(string fileExtension)
        {
            if(string.IsNullOrEmpty(fileExtension)) return false;
            if(allowedFilesExtenstions.Contains(fileExtension)) return true;
            return false;
        }
        private async Task<ResponseDTO> StoreStoryMediaAsync(IFormFile media)
        {
            if(media!=null)
            {
                var mediaName = Guid.NewGuid().ToString();
                var storyMediaPath = Path.Combine(_wwwrootPath, "StoryMedia");
                var fullPath = Path.Combine(storyMediaPath, mediaName);
                try
                {
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await media.CopyToAsync(fileStream);
                    }
                    return await _responseService.GenerateSuccessResponseAsync("Stored successfully", data:mediaName);
                }
                catch (Exception ex)
                {
                    if(File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
            return await _responseService.GenerateErrorResponseAsync("File not found");
        }
        private async Task<ResponseDTO> ViewStoryAsync(int storyId, int viewerId)
        {
            Story? story = await _storySpecificRepo.GetStoryWithUserAndViewersWithUsers(storyId);
            if (story == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Story not found");
            }
            AppUser viewer = await _appUserRepo.GetByIdAsync(viewerId);
            if (viewer == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Viewer not found");
            }
            StoryViewer? storyViewer = story.Viewers.Where(sv => sv.StoryId == storyId && sv.ViewerId == viewerId).FirstOrDefault();
            if (storyViewer == null)
            {
                StoryViewer newView = new StoryViewer { StoryId = story.Id, ViewedAt = DateTime.UtcNow, ViewerId = viewerId };
                await _storyViewerRepo.AddAsync(newView);
                await _storyViewerRepo.SaveChangesAsync();
            }
            return await _responseService.GenerateSuccessResponseAsync(data:story);
        }

        /* End of Helper Methods */
        public async Task<ResponseDTO> AddStoryAsync(StoryDTO story)
        {
            ValidationResult validationResult = await _storyDtoValidator.ValidateAsync(story);
            if (!validationResult.IsValid)
            {
                return await _responseService.GenerateErrorResponseAsync(validationResult.Errors.Select(err => err.ErrorMessage));
            }
            using (var transaction = await _storyRepo.BeginTransactionAsync())
            {
                try
                {
                    int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
                    Story newStory = new Story { Date = DateTime.UtcNow, UserId = currentUserId };
                    if (!string.IsNullOrEmpty(story.Content)) // Only content exists
                    {
                        newStory.Content = story.Content;
                    }
                    else // Only media exists
                    {
                        var fileExtenstion = Path.GetExtension(story.Media.FileName);
                        if (!CheckFileExtension(fileExtenstion))
                        {
                            return await _responseService.GenerateErrorResponseAsync("Invalid file type");
                        }
                        var storeResponse = await StoreStoryMediaAsync(story.Media);
                        if (!storeResponse.Success)
                        {
                            return storeResponse;
                        }
                        newStory.Media = storeResponse.Data as string;
                    }
                    await _storyRepo.AddAsync(newStory);
                    await transaction.CommitAsync();
                    await _storyRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Story added successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }

            
        }

        public async Task<ResponseDTO> GetFriendsStoriesAsync()
        {
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            AppUser currentUser = await _appUserRepo.FindWithIncludesAsync(u => u.Id == currentUserId, u => u.Friends);
            List<int> friendsIds = currentUser.Friends.Select(friendShip=>friendShip.FriendId==currentUserId?friendShip.UserId:friendShip.FriendId).ToList();
            if (friendsIds == null || friendsIds.Count == 0)
            {
                return await _responseService.GenerateSuccessResponseAsync("User has no friends");
            }
            var query = @"select * 
                        from stories s inner join aspnetusers u
                        on s.userid in ({0}) and s.userid = u.id";
            List<Story> friendsStories = await _storyRepo.FromSqlRawAsync(query,string.Join(",", friendsIds));
            var data = friendsStories
                .Select(s => new StoryViewDTO
                {
                    Content = s.Content,
                    DatePosted = s.Date,
                    Media = s.Media,
                    StoryId = s.Id,
                    UserFullName = s.User.FirstName + " " + s.User.LastName,
                    UserProfilePic = s.User.ProfilePic ?? string.Empty
                }
                );
            return await _responseService.GenerateSuccessResponseAsync(data: data);
        }

        public async Task<ResponseDTO> RemoveAllStoriesAsync()
        {
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            List<Story> stories = await _storyRepo.Where(s=>s.UserId == currentUserId).ToListAsync();
            using (var transaction = await _storyRepo.BeginTransactionAsync())
            {
                try
                {
                    foreach (var story in stories)
                    {
                        if (!string.IsNullOrEmpty(story.Media)) // Remove media if exists
                        {
                            var fullFilePath = Path.Combine(_wwwrootPath, "StoryMedia", story.Media);
                            if (File.Exists(fullFilePath))
                            {
                                File.Delete(fullFilePath);
                            }
                        }
                        _storyRepo.Remove(story);
                    }
                    await transaction.CommitAsync();
                    await _storyRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Deleted all stories successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
        }

        public async Task<ResponseDTO> RemoveStoryAsync(int storyId)
        {
            Story story = await _storyRepo.GetByIdAsync(storyId);
            if(story == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Story not found");
            }
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            if(story.UserId != currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            using (var transaction = await _storyRepo.BeginTransactionAsync())
            {
                try
                {
                    // First delete the media if exists
                    if (!string.IsNullOrEmpty(story.Media))
                    {
                        var fullFilePath = Path.Combine(_wwwrootPath, "StoryMedia", story.Media);
                        if (File.Exists(fullFilePath))
                        {
                            File.Delete(fullFilePath);
                        }
                    }
                    _storyRepo.Remove(story);
                    await transaction.CommitAsync();
                    await _storyRepo.SaveChangesAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Deleted successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
        }
        public async Task<ResponseDTO> ShowStoryAsync(int storyId, int viewerId)
        {
            var viewResponse = await ViewStoryAsync(storyId, viewerId);
            if(!viewResponse.Success)
            {
                return viewResponse;
            }
            Story story = viewResponse.Data as Story;

            var storyData = new StoryViewDTO
            {
                Content = story.Content,
                DatePosted = story.Date,
                Media = story.Media,
                StoryId = story.Id,
                UserFullName = story.User.FirstName + " " + story.User.LastName,
                UserProfilePic = story.User.ProfilePic ?? string.Empty
            };
            var data = new
            {
                storyInfo = storyData,
                viewData = new
                {
                    numberOfViews = story.Viewers.Count,
                    viewersData = story.Viewers.Select(sv=> new
                    {
                        viewerFullName = sv.Viewer.FirstName + " " + sv.Viewer.LastName,
                        viewerProfilePic = sv.Viewer.ProfilePic ?? string.Empty,
                        viewedAt = sv.ViewedAt,
                    })
                }

            };
            return await _responseService.GenerateSuccessResponseAsync(data: data);
        }
    }
}
