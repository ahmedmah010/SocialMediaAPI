using FluentValidation;
using FluentValidation.Results;
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
        private const long maxPostPhotoFileSize = 10*1024*1024; // 10 MB
        private const long maxPostVideoFileSize = 50*1024*1024; // 50 MB
        private readonly List<string> allowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".svg", ".mp4", ".avi" };
        private readonly string _wwwrootPath;

        private readonly IResponseService _responseService;
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<PostMedia> _postMediaRepo;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<PostDTO> _postDtoValidator;

        public PostService(IResponseService responseService,
                           IRepository<Post> postRepo,
                           ICurrentUserService currentUserService,
                           IValidator<PostDTO> postDtoValidator,
                           IRepository<PostMedia> postMediaRepo)
        {
            _responseService = responseService;
            _postRepo = postRepo;
            _currentUserService = currentUserService;
            _postDtoValidator = postDtoValidator;
            _postMediaRepo = postMediaRepo;
            _wwwrootPath = Path.GetFullPath("wwwroot");
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
            if (photos != null)
            {
                foreach (IFormFile photo in photos)
                {
                    if (photo.Length > maxPostPhotoFileSize)
                    {
                        return await _responseService.GenerateErrorResponseAsync($"Photo size must not exceed {maxPostPhotoFileSize / (1024*1024)}");
                    }
                }
            }
            if (videos != null)
            {
                foreach (IFormFile video in videos)
                {
                    if (video.Length > maxPostVideoFileSize)
                    {
                        return await _responseService.GenerateErrorResponseAsync($"Video size must not exceed {maxPostVideoFileSize / (1024 * 1024)}");
                    }
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
        private async Task<List<string>> SaveFilesAsync(List<IFormFile>? files, int postId)
        {
            List<string> fileNames = new List<string>();
            string wwwrootPath = _wwwrootPath;
            if(files!=null && files.Count > 0)
            {
                var fileUploadTasks = files.Select(async file =>
                {
                    var mainUploadsFolderPath = Path.Combine(wwwrootPath, "PostMedia");
                    var postFolderName = postId.ToString();
                    var uploadPath = Path.Combine(mainUploadsFolderPath, postFolderName);
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    var filePath = Path.Combine(uploadPath, uniqueFileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return uniqueFileName;
                });
                fileNames = (await Task.WhenAll(fileUploadTasks)).ToList();
            }
            return fileNames;
        }
        private List<IFormFile> CombineFiles(List<IFormFile> photos, List<IFormFile> videos)
        {
            List<IFormFile> files = new List<IFormFile>();
            if (photos != null)
            {
                files.AddRange(photos);
            }
            if (videos != null)
            {
                files.AddRange(videos);
            }
            return files;
        }
        private async Task<(ResponseDTO, List<IFormFile>?)> CheckFilesAsync(List<IFormFile> photos, List<IFormFile> videos)
        {
            ResponseDTO response;
            List<IFormFile> files = new List<IFormFile>();
            try
            {
                // Check if the number of uploaded files is in the acceptable range
                response = await CheckNumberOfUploadedFilesAsync(photos, videos);
                if (!response.Success)
                {
                    return (response, null);
                }
                // Check if files extentions are correct
                files = CombineFiles(photos, videos);
                response = await CheckFileExtensionAsync(files, allowedFileExtensions);
                if (!response.Success)
                {
                    return (response, null);
                }
                // Check if the size is appropriate
                response = await CheckFileSizeAsync(photos, videos);
                if (!response.Success)
                {
                    return (response, null);
                }
            }
            catch (Exception ex)
            {
                return (await _responseService.GenerateErrorResponseAsync(ex.Message), null);
            }

            return (await _responseService.GenerateSuccessResponseAsync("File(s) valid"), files);
        }
        /* End of helper methods */

        public async Task<ResponseDTO> CreatePostAsync(PostDTO postDTO)
        {
            // Apply some kind of validator to some fields using FluentValidation
            ValidationResult validationResult = await _postDtoValidator.ValidateAsync(postDTO);
            if(!validationResult.IsValid)
            {
                return await _responseService.GenerateErrorResponseAsync(validationResult.Errors.Select(err=>err.ErrorMessage));
            }
            List<IFormFile> combinedFiles = new List<IFormFile>();
            if (postDTO.Photos != null || postDTO.Videos != null)
            {
                var (response, responseFiles) = await CheckFilesAsync(postDTO.Photos, postDTO.Videos);
                if (!response.Success)
                { return response; }
                combinedFiles = responseFiles;
            }
            using (var transaction = await _postRepo.BeginTransactionAsync())
            {
                try
                {
                    // Save the post to the db first in order to get an Id 
                    Post newPost = new Post
                    {
                        Date = DateTime.Now,
                        Privacy = postDTO.Privacy,
                        Content = postDTO.Content,
                        UserId = await _currentUserService.GetCurrentUserIdAsync()
                    };
                    newPost.ReactionsStatus = new ReactionsStatus();
                    await _postRepo.AddAsync(newPost);
                    await _postRepo.SaveChangesAsync();
                    postDTO.Id = newPost.Id;
                    if (postDTO.Photos != null || postDTO.Videos != null)
                    {
                        // Call a function that saves the photos and videos inside a folder with the name = postId
                        List<string> fileNames = await SaveFilesAsync(combinedFiles, newPost.Id);
                        if (fileNames != null && fileNames.Count > 0)
                        {
                            List<PostMedia> postMedia = fileNames.Select(fn => new PostMedia { PostId = newPost.Id, Url = fn }).ToList();
                            newPost.Media = postMedia;
                            await _postRepo.SaveChangesAsync();
                        }
                    }
                } 
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);

                }
                await transaction.CommitAsync();
            }
            // Return a postDTO that represents the added post
            return await _responseService.GenerateSuccessResponseAsync("Post added successfully", postDTO);
        }

        public async Task<ResponseDTO> UpdatePostAsync(PostDTO postDTO)
        {
            //validate the post properties
            ValidationResult validationResult = await _postDtoValidator.ValidateAsync(postDTO);
            if(!validationResult.IsValid)
            {
                return await _responseService.GenerateErrorResponseAsync(validationResult.Errors.Select(err => err.ErrorMessage));
            }
            if(postDTO.Id == 0) 
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid post id");
            }
            Post post = await _postRepo.FindWithIncludesAsync(p => p.Id == postDTO.Id, p => p.Media);
            if(post is null)
            {
                return await _responseService.GenerateErrorResponseAsync("Post not found");
            }
            //check if there are uploaded files, if so then check them
            List<IFormFile> combinedFiles = new List<IFormFile>();
            using (var transaction = await _postRepo.BeginTransactionAsync())
            {
                if (postDTO.Photos != null || postDTO.Videos != null)
                {
                    var (response, responseFiles) = await CheckFilesAsync(postDTO.Photos, postDTO.Videos);
                    if (!response.Success)
                    { return response; }
                    combinedFiles = responseFiles;

                    /* Now, in order to remove the media user removed and add the new media the user has added, we will do the following approach
                    First check for the media in the db, if there is a media that doesn't present in the new list of media then delete these records 
                    and their files in the PostMedia. */
                    // I selected the filename only from the combinedFiles using Select, then convered it into ToList so that I can access the method Contains()
                    try
                    {
                        List<string> givenFilesNames = combinedFiles.Select(f => f.FileName).ToList();
                        List<PostMedia> mediaToDelete = post.Media.Where(m => !givenFilesNames.Contains(m.Url)).ToList();
                        mediaToDelete.ForEach(m =>
                        {
                            _postMediaRepo.Remove(m); // delete the file from the db
                            var filePah = Path.Combine(_wwwrootPath, "PostMedia", m.Url);
                            if (File.Exists(filePah))
                            {
                                File.Delete(filePah); // remove the actual file from the server
                            }
                        }
                        );
                        /* Second, check for the files in the list, if there are files that don't present in the db, 
                        then add them to the db */
                        List<string> postMediaUrls = post.Media.Select(m => m.Url).ToList();
                        List<IFormFile> mediaToAdd = combinedFiles.Where(f => !postMediaUrls.Contains(f.FileName)).ToList();
                        // Now add them
                        var mediaToAddUrls = await SaveFilesAsync(mediaToAdd, post.Id);
                        foreach (var url in mediaToAddUrls)
                        {
                            post.Media.Add(new PostMedia { PostId = post.Id, Url = url });
                        }
                    }
                    catch(Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return await _responseService.GenerateErrorResponseAsync(ex.Message);
                    }
                }
                //Finaly update post privacy, content, etc.
                post.Content = postDTO.Content;
                post.Privacy = postDTO.Privacy;
                await _postRepo.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            return await _responseService.GenerateSuccessResponseAsync("Post has been updated", postDTO);

        }
        public async Task<ResponseDTO> RemovePostAsync(int id)
        {
            if (id == 0)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid id");
            }
            Post post = await _postRepo.GetByIdAsync(id);
            if (post == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Post not found");
            }
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            if (post.UserId != currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("Access denied");
            }
            // First delete the directory that contains all the post media, then delete the post itself
            // And as the deletion is cascaded, all relative entities to the post will be also delete e.g. PostMedia, PostComments, etc.
            using (var transaction = await _postRepo.BeginTransactionAsync())
            {
                try
                {
                    var postMediaPath = Path.Combine(_wwwrootPath, "PostMedia", post.Id.ToString());
                    if (Directory.Exists(postMediaPath))
                    {
                        Directory.Delete(postMediaPath, true);
                    }
                    // First manually remove the associated Reactions and ReactionsStatuses then
                    // Delete all the ReactionStatuses and Reactions corresponding to the comments on this post
                    var query = @"
                            delete from Reactions where PostId = {0};
                            delete from ReactionsStatuses where postId = {0};
                            select id into #TempIds
                            from comments
                            where postid = {0} 

                            delete from Reactions
                            where commentid in (select * from #TempIds)
                            delete from ReactionsStatuses
                            where commentid in (select * from #TempIds)
                            DROP TABLE #TempIds";
                    await _postRepo.ExecuteSqlRawAsync(query, post.Id);
                    // Delete the post itself and also the comments associated to it will be automatically deleted (delte on cascade)
                    _postRepo.Remove(post);
                    await _postRepo.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
            return await _responseService.GenerateSuccessResponseAsync("Post has been deleted");
        }
    }
}
