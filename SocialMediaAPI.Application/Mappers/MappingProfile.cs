using AutoMapper;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDTO, AppUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<FriendRequest, ReceivedFriendRequestDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Requester.FirstName + " " + src.Requester.LastName))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Requester.UserName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<FriendRequest, SentFriendRequestDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Receiver.FirstName + " " + src.Receiver.LastName))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Receiver.UserName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<AppUser, FriendDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.ProfilePic));
        }
    }
}
