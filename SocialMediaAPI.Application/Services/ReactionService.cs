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
        /*
        public async Task<ResponseDTO> AddReactionAsync<TEntity,TReaction>(TEntity entity, 
            TReaction entityReaction,
            ReactionType entityReactionType,
            ReactionType reactionType,
            IRepository<TEntity> _entityRepo,
            Func<TEntity, ReactionsStatus> GetReactionStatus,
            Func<TEntity, ICollection<TReaction>> GetReactions,
            Func<TReaction> CreateReaction,
            Action<ReactionType> ChangeCurrentReactionType
            ) 
            where TReaction : class
            where TEntity : class
        {
            var reactionStatus = GetReactionStatus(entity);
            var reactions = GetReactions(entity);
            using (var transaction = await _entityRepo.BeginTransactionAsync())
            {
                try
                {
                    if (entityReaction == null) // First time this particular current user reacts 
                    {
                        entityReaction = CreateReaction();
                        reactions.Add(entityReaction);
                        UpdateReactionsStatus(reactionStatus, reactionType, ReactionType.None);
                        await _entityRepo.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return await _responseService.GenerateSuccessResponseAsync($"{typeof(TEntity).Name} reaction has been added successfully");
                    }
                    else // User has already reacted, maybe he is trying to change the reaction
                    {
                        if (entityReactionType == reactionType)
                        {
                            return await _responseService.GenerateErrorResponseAsync($"You've already reacted to this {typeof(TEntity).Name.ToLower()}");
                        }
                        UpdateReactionsStatus(GetReactionStatus(entity), reactionType, entityReactionType);
                        //entityReaction.ReactionType = reactionType;
                        ChangeCurrentReactionType(reactionType);
                        await transaction.CommitAsync();
                        await _entityRepo.SaveChangesAsync();
                        return await _responseService.GenerateSuccessResponseAsync($"{typeof(TEntity).Name} reaction has been added successfully");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }
            }
        }*/
        public async Task<ResponseDTO> AddCommentReactionAsync(int commentId, ReactionType reactionType)
        {
            //check if comment exists
            //check if a reaction to this comment with the current user id is null then create a new reaction object and fill the required info
            //if the reaction exists, chec if the type has changed then just update it
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            Comment comment = await _commentRepo.FindWithIncludesAsync(c=>c.Id == commentId, c=>c.ReactionsStatus, c=>c.Reactions.Where(cr=>cr.UserId==currentUserId));
            if (comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment doesn't exist");
            }
            CommentReaction? commentReaction = comment.Reactions.FirstOrDefault();
            using (var transaction = await _commentRepo.BeginTransactionAsync())
            {
                try
                {
                    if (commentReaction == null) // First time this particular current user reacts 
                    {
                        commentReaction = new CommentReaction { CommentId = commentId, UserId = currentUserId, ReactionType = reactionType };
                        comment.Reactions.Add(commentReaction);
                        UpdateReactionsStatus(comment.ReactionsStatus, reactionType, ReactionType.None);
                        await _commentRepo.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return await _responseService.GenerateSuccessResponseAsync("Comment reaction has been added successfully");
                    }
                    else // User has already reacted, maybe he is trying to change the reaction
                    {
                        if (commentReaction.ReactionType == reactionType)
                        {
                            return await _responseService.GenerateErrorResponseAsync("You've already reacted to this comment");
                        }
                        UpdateReactionsStatus(comment.ReactionsStatus, reactionType, commentReaction.ReactionType);
                        commentReaction.ReactionType = reactionType;
                        await transaction.CommitAsync();
                        await _commentRepo.SaveChangesAsync();
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

        public async Task<ResponseDTO> AddPostReactionAsync(int postId, ReactionType reactionType)
        {
            int currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            Post post = await _postRepo.FindWithIncludesAsync(p => p.Id == postId, c => c.ReactionsStatus, c => c.Reactions.Where(pr => pr.UserId == currentUserId));
            if (post == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Post doesn't exist");
            }
            PostReaction? postReaction = post.Reactions.FirstOrDefault();
            using (var transaction = await _postRepo.BeginTransactionAsync())
            {
                try
                {
                    if (postReaction == null) // First time this particular current user reacts 
                    {
                        postReaction = new PostReaction { PostId = postId, UserId = currentUserId, ReactionType = reactionType };
                        post.Reactions.Add(postReaction);
                        UpdateReactionsStatus(post.ReactionsStatus, reactionType, ReactionType.None);
                        await _commentRepo.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return await _responseService.GenerateSuccessResponseAsync("Post reaction has been added successfully");
                    }
                    else // User has already reacted, maybe he is trying to change the reaction
                    {
                        if (postReaction.ReactionType == reactionType)
                        {
                            return await _responseService.GenerateErrorResponseAsync("You've already reacted to this post");
                        }
                        UpdateReactionsStatus(post.ReactionsStatus, reactionType, postReaction.ReactionType);
                        postReaction.ReactionType = reactionType;
                        await transaction.CommitAsync();
                        await _postRepo.SaveChangesAsync();
                        return await _responseService.GenerateSuccessResponseAsync("Post reaction has been added successfully");
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return await _responseService.GenerateErrorResponseAsync(ex.Message);
                }

            }
        }
        private void ExtractReactions(ReactionsStatus reactionsStatus, List<ReactionType> reactions)
        {
            if (reactionsStatus.HahaCount > 0)
            {
                reactions.Add(ReactionType.Haha);
            }
            if (reactionsStatus.AngryCount > 0)
            {
                reactions.Add(ReactionType.Angry);
            }
            if (reactionsStatus.LikeCount > 0)
            {
                reactions.Add(ReactionType.Like);
            }
            if (reactionsStatus.LoveCount > 0)
            {
                reactions.Add(ReactionType.Love);
            }
        }

        public async Task<ResponseDTO> GetCommentReactionsAsync(int commentId)
        {
            Comment comment = await _commentRepo.GetByIdAsync(commentId);
            if(comment == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Comment not found");
            }
            ReactionsStatus reactionsStatus = await _reactionsStatusRepo.FirstOrDefaultAsync(rs=>rs.CommentId == commentId);
            List<ReactionType> reactions = new List<ReactionType>();
            ExtractReactions(reactionsStatus, reactions);
            return await _responseService.GenerateSuccessResponseAsync(data:reactions);
        }

        public async Task<ResponseDTO> GetPostReactionsAsync(int postId)
        {
            Post post = await _postRepo.GetByIdAsync(postId);
            if (post == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Post not found");
            }
            ReactionsStatus reactionsStatus = await _reactionsStatusRepo.FirstOrDefaultAsync(rs => rs.PostId == postId);
            List<ReactionType> reactions = new List<ReactionType>();
            ExtractReactions(reactionsStatus, reactions);
            return await _responseService.GenerateSuccessResponseAsync(data: reactions);
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
                    await transaction.CommitAsync();
                    await _reactionsStatusRepo.SaveChangesAsync();
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
