using Microsoft.AspNetCore.Http;
using SocialMediaAPI.Application.DTOs;
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
    public class PostService : IPostService
    {
        private const int allowedNumberOfPostPhotos = 10;
        private const int allowedNumberOfPostVideos = 3;
        private const long maxPostPhotoFileSize = 5*1024*1024; // 5 MB
        private const long maxPostVideoFileSize = 30*1024*1024; // 30 MB
        private readonly List<string> allowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".svg", ".mp4", ".avi" };

        private readonly IResponseService _responseService;
        private readonly IRepository<Post> _postRepo;
        private readonly ICurrentUserService _currentUserService;

        public PostService(IResponseService responseService,
                           IRepository<Post> postRepo,
                           ICurrentUserService currentUserService)
        {
            _responseService = responseService;
            _postRepo = postRepo;
            _currentUserService = currentUserService;
        }
        /* Helper methods */
        private async Task<ResponseDTO> CheckFileExtensionAsync(List<IFormFile> files, List<string> fileExtensions)
        {
            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    if (!fileExtensions.Contains(Path.GetExtension(file.FileName)))
                    {
                        return await _responseService.GenerateErrorResponseAsync("Invalid file type");
                    }
                }
                return await _responseService.GenerateSuccessResponseAsync();
            }
            return await _responseService.GenerateErrorResponseAsync("There're no files to check");
        }
        private async Task<ResponseDTO> CheckFileSizeAsync(List<IFormFile> photos, List<IFormFile> videos)
        {
            if(photos == null && videos == null)
            {
                return await _responseService.GenerateErrorResponseAsync("There're no files to check");
            }
            foreach(IFormFile photo in photos)
            {
                if(photo.Length > maxPostPhotoFileSize)
                {
                    return await _responseService.GenerateErrorResponseAsync($"Photo size must not exceed {maxPostPhotoFileSize}");
                }
            }
            foreach(IFormFile video in videos)
            {
                if(video.Length > maxPostVideoFileSize)
                {
                    return await _responseService.GenerateErrorResponseAsync($"Video size must not exceed {maxPostVideoFileSize}");
                }
            }
            return await _responseService.GenerateSuccessResponseAsync();
        }
        private async Task<ResponseDTO> CheckNumberOfUploadedFilesAsync(List<IFormFile> photos, List<IFormFile> videos)
        {
            if (photos == null && videos == null)
            {
                return await _responseService.GenerateErrorResponseAsync("There're no files to check");
            }
            if(photos!=null && photos.Count > allowedNumberOfPostPhotos)
            {
                return await _responseService.GenerateErrorResponseAsync($"Number of uploaded photos exceeds the allowed number of {allowedNumberOfPostPhotos}");
            }
            if(videos!=null && videos.Count > allowedNumberOfPostVideos)
            {
                return await _responseService.GenerateErrorResponseAsync($"Number of uploaded videos exceeds the allowed number of {allowedNumberOfPostVideos}");
            }
            return await _responseService.GenerateSuccessResponseAsync();
        }
        private async Task<List<string>> SaveFilesAsync(List<IFormFile> files, int postId)
        {
            List<string> fileNames = new List<string>();
            string wwwrootPath = Path.GetFullPath("wwwroot");
            if(files!=null && files.Count > 0)
            {
                foreach (IFormFile file in files)
                {
                    var mainUploadsFolderPath = Path.Combine(wwwrootPath, "PostMedia");
                    var postFolderPath = postId.ToString();
                    var uploadPath = Path.Combine(mainUploadsFolderPath, postFolderPath);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadPath, uniqueFileName);
                    using(FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    fileNames.Add(Path.Combine(postFolderPath, uniqueFileName));
                }
            }
            return fileNames;
        }
        /* End of helper methods */

        public async Task<ResponseDTO> CreatePostAsync(PostDTO postDTO)
        {
            // Apply some kind of validator to some fields?
            ResponseDTO response;
            // Save the post to the db first in order to get an Id
            Post newPost = new Post
            {
                Date = DateTime.Now,
                Privacy = PostPrivacy.Public,
                Content = postDTO.Content,
                UserId = await _currentUserService.GetCurrentUserIdAsync()
            };
            await _postRepo.AddAsync(newPost);
            if (postDTO.Photos != null || postDTO.Videos != null)
            {
                // Check if the number of uploaded files is in the acceptable range
                response = await CheckNumberOfUploadedFilesAsync(postDTO.Photos, postDTO.Videos);
                if (!response.Success)
                {
                    return response;
                }
                // Check if files extentions are correct
                List<IFormFile> files = new List<IFormFile>();
                if (postDTO.Photos != null)
                {
                    files.AddRange(postDTO.Photos);
                }
                if (postDTO.Videos != null)
                {
                    files.AddRange(postDTO.Videos);
                }
                response = await CheckFileExtensionAsync(files, allowedFileExtensions);
                if (!response.Success)
                {
                    return response;
                }
                // Check if the size is appropriate
                response = await CheckFileSizeAsync(postDTO.Photos, postDTO.Videos);
                if (!response.Success)
                {
                    return response;
                }
                // Call a function that saves the photos and videos inside a folder with the name = PostId
                List<string> fileNames = await SaveFilesAsync(files, newPost.Id);
                if (fileNames != null && fileNames.Count > 0)
                {
                    List<PostMedia> postMedia = fileNames.Select(fn => new PostMedia { PostId = newPost.Id, Url = fn }).ToList();
                    newPost.Media = postMedia;
                }
            }
            await _postRepo.SaveChangesAsync();
            //you should return a postDTO that represents the added post

            return await _responseService.GenerateSuccessResponseAsync();
        }
    }
}
