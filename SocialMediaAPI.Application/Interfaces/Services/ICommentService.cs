using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<ResponseDTO> AddPostCommentAsync(int postId, string content);
        Task<ResponseDTO> AddReplyToCommentAsync(int parentCommentId, string content);
        Task<ResponseDTO> UpdateCommentAsync(int id, string content);
        Task<ResponseDTO> RemoveCommentAsync(int id);
    }
}
