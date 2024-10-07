using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<ResponseDTO> CreatePostAsync(PostDTO post);
    }
}
