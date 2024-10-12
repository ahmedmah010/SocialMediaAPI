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
    public class ReactionService : IReactionService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<ReactionsStatus> _reactionsStatusRepo;
        private readonly IRepository<Comment> _commentRepo;
        private readonly IRepository<CommentReaction> _commentReactionRepo;
        private readonly IRepository<PostReaction> _postReactionRepo;
        private readonly IRepository<Post> _postRepo;
        private readonly IResponseService _responseService;
        public ReactionService(ICurrentUserService currentUserService, 
            IRepository<ReactionsStatus> reactionsStatusRepo, 
            IRepository<Comment> commentRepo,
            IRepository<Post> postRepo,
            IResponseService responseService,
            IRepository<CommentReaction> commentReactionRepo,
            IRepository<PostReaction> postReactionRepo)
        {
            _currentUserService = currentUserService;
            _reactionsStatusRepo = reactionsStatusRepo;
            _commentRepo = commentRepo;
            _postRepo = postRepo;
            _responseService = responseService;
            _commentReactionRepo = commentReactionRepo;
            _postReactionRepo = postReactionRepo;
        }
        private void UpdateReactionsStatus(ReactionsStatus reactionsStatus, ReactionType newReactionType, ReactionType oldReactionType)
        {
            if (newReactionType != oldReactionType)
            {
                switch (newReactionType)
                {
                    case ReactionType.Angry: reactionsStatus.AngryCount++; break;
                    case ReactionType.Haha: reactionsStatus.HahaCount++; break;
                    case ReactionType.Like: reactionsStatus.LikeCount++; break;
                    case ReactionType.Love: reactionsStatus.LoveCount++; break;
                }
                switch (oldReactionType)
                {
                    case ReactionType.Angry: reactionsStatus.AngryCount--; break;
                    case ReactionType.Haha: reactionsStatus.HahaCount--; break;
                    case ReactionType.Like: reactionsStatus.LikeCount--; break;
                    case ReactionType.Love: reactionsStatus.LoveCount--; break;
                }
                reactionsStatus.UpdateTotalReactionsCount();
            }
        }
        public async Task<ResponseDTO> AddCommentReactionAsync(int commentId, ReactionType reactionType)
        {
            //check if comment exists
            //check if a reaction to this comment with the current user id is null then create a new reaction object and fill the required info
            //if the reaction exists, chec if the type has changed then just update it
            Comment comment = await _commentRepo.GetByIdAsync(commentId);
            if (comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment doesn't exist");
            }
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            ReactionsStatus reactionStatus = await _reactionsStatusRepo.FirstOrDefaultAsync(rs => rs.CommentId == commentId);
            CommentReaction commentReaction = await _commentReactionRepo.FirstOrDefaultAsync(cr => cr.UserId == currentUserId && cr.CommentId == commentId);
            using (var transaction = await _commentReactionRepo.BeginTransactionAsync())
            {
                try
                {
                    if (commentReaction == null) // First time this particular current user reacts 
                    {
                        commentReaction = new CommentReaction { CommentId = commentId, UserId = currentUserId, ReactionType = reactionType };
                        await _commentReactionRepo.AddAsync(commentReaction);
                        UpdateReactionsStatus(reactionStatus, reactionType, ReactionType.None);
                        await _reactionsStatusRepo.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return await _responseService.GenerateSuccessResponseAsync("Comment reaction has been added successfully");
                    }
                    else // User has already reacted, maybe he is trying to change the reaction
                    {
                        if (commentReaction.ReactionType == reactionType)
                        {
                            return await _responseService.GenerateErrorResponseAsync("You've already reacted to this comment");
                        }
                        UpdateReactionsStatus(reactionStatus, reactionType, commentReaction.ReactionType);
                        commentReaction.ReactionType = reactionType;
                        await _reactionsStatusRepo.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return await _responseService.GenerateSuccessResponseAsync("Comment reaction has been added successfully");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }

            }
        }

        public Task<ResponseDTO> AddPostReaction(int postId, ReactionType reactionType)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> GetCommentReactions(int commentId)
        {
            Comment comment = await _commentRepo.GetByIdAsync(commentId);
            if(comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment not found");
            }

            throw new NotImplementedException();
        }

        public Task<ResponseDTO> GetPostReactions(int postId)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseDTO> RemoveReactionAsync(int id)
        {
            var postReaction = await _postReactionRepo.GetByIdAsync(id);
            if(postReaction != null)
            {
                return  await ReactionRemovalHandlerAsync(postReaction);
            }
            var commentReaction = await _commentReactionRepo.GetByIdAsync(id);
            if (commentReaction != null)
            {
                return await ReactionRemovalHandlerAsync(commentReaction);
            }
            return await _responseService.GenerateErrorResponseAsync("Reaction not found");
        }
        private async Task<ResponseDTO> ReactionRemovalHandlerAsync(ReactionBase reaction)
        {
            // check if reaction belongs to the current user
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            if(reaction.UserId!= currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("Invalid operation");
            }
            using (var transaction = await _reactionsStatusRepo.BeginTransactionAsync())
            {
                try
                {
                    var reactionStatus = await _reactionsStatusRepo.FirstOrDefaultAsync(rs => reaction.IsPostReaction ? rs.PostId == ((PostReaction)reaction).PostId : rs.CommentId == ((CommentReaction)reaction).CommentId);
                    UpdateReactionsStatus(reactionStatus, ReactionType.None, reaction.ReactionType); // Decrements the reaction to be deleted
                    if (reaction.IsPostReaction)
                    {
                        _postReactionRepo.Remove((PostReaction)reaction);
                    }
                    else
                    {
                        _commentReactionRepo.Remove((CommentReaction)reaction);
                    }
                    await _reactionsStatusRepo.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return await _responseService.GenerateSuccessResponseAsync("Reaction removed");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }

        }
    }
}
