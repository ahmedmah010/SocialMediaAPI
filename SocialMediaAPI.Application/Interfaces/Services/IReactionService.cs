using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IReactionService
    {
        Task<ResponseDTO> AddPostReactionAsync(int postId, ReactionType reactionType);
        Task<ResponseDTO> AddCommentReactionAsync(int commentId, ReactionType reactionType);
        Task<ResponseDTO> RemoveReactionAsync(int id);
        Task<ResponseDTO> GetPostReactions(int postId);
        Task<ResponseDTO> GetCommentReactions(int commentId);
    }
}
