using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IResponseService _responseService;
        private readonly IRepository<Post> _postRepo;
        private readonly IRepository<Comment> _commentRepo;
        public CommentService(ICurrentUserService currentUserService, 
            IResponseService responseService,
            IRepository<Post> postRepo,
            IRepository<Comment> commentRepo)
        {
            _currentUserService = currentUserService;
            _responseService = responseService;
            _postRepo = postRepo;
            _commentRepo = commentRepo;
        }

        public async Task<ResponseDTO> AddPostCommentAsync(int postId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return await _responseService.GenerateErrorResponseAsync("You can't add an empty comment");
            }
            if(postId == 0)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid id");
            }
            Post post = await _postRepo.GetByIdAsync(postId);
            if (post == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Post not found");
            }
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            Comment newComment = new Comment { Content = content, Date = DateTime.Now, PostId = postId, UserId = currentUserId };
            newComment.ReactionsStatus = new ReactionsStatus();
            await _commentRepo.AddAsync(newComment);
            await _commentRepo.SaveChangesAsync();
            return await _responseService.GenerateSuccessResponseAsync("Comment added successfully");
        }
        public async Task<ResponseDTO> AddReplyToCommentAsync(int parentCommentId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return await _responseService.GenerateErrorResponseAsync("You can't add an empty comment");
            }
            if (parentCommentId == 0)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid id");
            }
            Comment parentComment = await _commentRepo.GetByIdAsync(parentCommentId);
            if (parentComment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment not found");
            }
            int currentPostId = parentComment.PostId;
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            Comment newChildComment = new Comment 
            { 
                PostId = currentPostId,
                ParentCommentId = parentCommentId,
                UserId = currentUserId,
                Content = content,
                Date = DateTime.Now
            };
            newChildComment.ReactionsStatus = new ReactionsStatus();
            await _commentRepo.AddAsync(newChildComment);
            await _commentRepo.SaveChangesAsync();
            return await _responseService.GenerateSuccessResponseAsync("Reply added successfully");
        }
        public async Task<ResponseDTO> UpdateCommentAsync(int id, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return await _responseService.GenerateErrorResponseAsync("You can't add an empty comment");
            }
            if (id == 0)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid id");
            }
            Comment comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment not found");
            }
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            if(comment.UserId != currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            if (comment.Content != content) // To avoid unnecessary database updates
            {
                comment.Content = content;
                await _commentRepo.SaveChangesAsync();
            }
            return await _responseService.GenerateSuccessResponseAsync("Comment updated successfully");
        }

        public async Task<ResponseDTO> RemoveCommentAsync(int id)
        {
            if (id == 0)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid id");
            }
            Comment comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment not found");
            }
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            int currentPostOwnerId = (await _postRepo.GetByIdAsync(comment.PostId)).UserId;
            if (comment.UserId != currentUserId && currentPostOwnerId != currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            using (var transaction = await _commentRepo.BeginTransactionAsync())
            {
                try
                {
                    // Recursive CTE query to delete the parent comment with all its descendants (if exist), also removes the corresponding Reactions and ReactionStatus
                    string query = @"with descendants as (
                                select Id from comments where Id = {0} 
                                union all
                                select c.Id 
                                from comments c inner join descendants d
                                on c.ParentCommentId = d.Id
                            )
                            -- Create a temp table to store the values 
                            select * into #TempIds from descendants
                            -- Delete reactions associated with the comments
                            delete from Reactions
                            where CommentId in (select id from #TempIds)
                            -- Delete ReactionsStatuses associated with the comments
                            delete from ReactionsStatuses
                            where CommentId in (select id from #TempIds)
                            -- Delete the comments themselves
                            delete from comments
                            where id in (select id from #TempIds)
                            -- Clean up the temp table
                            DROP TABLE #TempIds";
                    await _commentRepo.ExecuteSqlRawAsync(query, comment.Id);
                    await transaction.CommitAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Comment deleted successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
        }
        // This method is not used, as it is replaced with a raw SQL query for performance and efficiency.
        // The method is works as expected, it was implemented for learning purposes
        private async Task GetAllRepliesRecursivelyAsync(int id, List<Comment> AllReplies)
        {
            var childComments = (await _commentRepo.FindWithIncludesAsync(c=>c.Id == id, c=>c.ChildComments)).ChildComments;
            if (childComments != null)
            {
                AllReplies.AddRange(childComments);
                foreach (var childComment in childComments)
                {
                    await GetAllRepliesRecursivelyAsync(childComment.Id, AllReplies);

                }
            }
        }

       
    }
}
