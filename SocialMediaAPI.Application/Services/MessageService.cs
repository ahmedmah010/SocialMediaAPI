using Microsoft.AspNetCore.Identity;
using SocialMediaAPI.Application.DTOs;
using SocialMediaAPI.Application.DTOs.Response;
using SocialMediaAPI.Application.Interfaces.Services;
using SocialMediaAPI.Domain.Entities;
using SocialMediaAPI.Domain.Repositories;
using SocialMediaAPI.Domain.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IResponseService _responseService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepository<Message> _messageRepo;
        private readonly IRepository<Chat> _chatRepo;
        private readonly IChatRepo _chatSpecificRepo;
        private readonly UserManager<AppUser> _userManager;

        public MessageService(IResponseService responseService,
            ICurrentUserService currentUserService,
            IRepository<Message> messageRepo,
            UserManager<AppUser> userManager,
            IRepository<Chat> chatRepo,
            IChatRepo chatSpecificRepo)
        {
            _responseService = responseService;
            _currentUserService = currentUserService;
            _messageRepo = messageRepo;
            _userManager = userManager;
            _chatRepo = chatRepo;
            _chatSpecificRepo = chatSpecificRepo;
        }
        public async Task<ResponseDTO> GetChatsAsync(int pageNumber = 1, int pageSize = 5) // Gets all the conversations
        {
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            int skipCount = (pageNumber - 1) * pageSize;
            List<Chat> chats = await _chatSpecificRepo.GetChatsWithLastMessageAsync(currentUserId, pageNumber, pageSize);
            List<ChatHeadDTO> chatHeads = new List<ChatHeadDTO>();
            if(chats != null)
            {
                foreach (Chat chat in chats)
                {
                    var receiverId = chat.Participant1Id == currentUserId ? chat.Participant2Id : chat.Participant1Id;
                    var receiver = await _userManager.FindByIdAsync(receiverId.ToString());
                    Message lastMsg = chat.Messages.FirstOrDefault();
                    chatHeads.Add
                        (
                        new ChatHeadDTO
                        {
                            ChatId = chat.Id,
                            LastMsg = lastMsg.Content,
                            LastMsgDate= lastMsg.SentAt,
                            ReceiverFullName = receiver.FirstName + " " + receiver.LastName, ReceiverProfilePicPath = receiver.ProfilePic 
                        }
                        );
                }
            }
            int totalChatsCount = await _chatRepo.CountAsync();
            bool hasMoreChats = (totalChatsCount > (skipCount + pageSize));
            var responseData = new
            {
                Data = chatHeads,
                HasMoreChats = hasMoreChats
            };
            return await _responseService.GenerateSuccessResponseAsync(data: responseData);
        }
        public async Task<ResponseDTO> GetChatAsync(int chatId, int pageNumber = 1, int pageSize = 20) // Gets specific chat history
        {
            Chat chat = await _chatRepo.GetByIdAsync(chatId);
            if (chat == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Chat not found");
            }
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            int skipCount = (pageNumber - 1) *pageSize;
            List<MessageDTO> messages = _messageRepo.Where(m => m.ChatId == chatId).Include(m => m.Sender).OrderByDesc(m => m.SentAt)
                .Skip(skipCount).Take(pageSize)
                .Select(m=> new MessageDTO 
                { 
                    Content = m.Content,
                    IsSeen = m.IsSeen,
                    MessageId = m.Id,
                    SentAt = m.SentAt,
                    UserId = m.SenderId,
                    FullName = m.Sender.FirstName + " " + m.Sender.LastName,
                    ProfileImagePath = m.Sender.ProfilePic ?? string.Empty,
                }
                ).ToList();
            int totalMessagesCount = await _messageRepo.CountAsync(m => m.ChatId == chatId);
            bool hasMoreMessages = (totalMessagesCount > (skipCount + pageSize));
            var responseData = new
            {
                Data = messages,
                HasMoreMessages = hasMoreMessages
            };
            return await _responseService.GenerateSuccessResponseAsync(data: responseData);
        }

        public async Task<ResponseDTO> MarkMessageAsSeenAsync(List<int> messagesIds)
        {
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            var msgsIds = string.Join(",", messagesIds);
            var query = @"UPDATE Messages SET IsSeen = 1 WHERE id in ({0}) AND ReceiverId = {1}";
            await _messageRepo.ExecuteSqlRawAsync(query, msgsIds,currentUserId);
            return await _responseService.GenerateSuccessResponseAsync();
        }

        public async Task<ResponseDTO> SendMessageAsync(MessageDTO message, int receiverId)
        {
            var currentUserId = await _currentUserService.GetCurrentUserIdAsync();
            if (receiverId == currentUserId)
            {
                return await _responseService.GenerateErrorResponseAsync("You can't message yourself");
            }
            if (string.IsNullOrEmpty(message.Content))
            {
                return await _responseService.GenerateErrorResponseAsync("Cannot send an empty message");
            }
            AppUser receiver = await _userManager.FindByIdAsync(receiverId.ToString());
            if (receiver == null)
            {
                return await _responseService.GenerateErrorResponseAsync("Receiver not found");
            }
            Chat chat = await _chatRepo.Where(c => (c.Participant1Id == currentUserId && c.Participant2Id == receiverId) ||
                                                   (c.Participant1Id == receiverId && c.Participant2Id == currentUserId)).FirstOrDefaultAsync();
            if (chat == null) // First time to chat
            {
                chat = new Chat { Participant1Id = currentUserId, Participant2Id = receiverId};
                await _chatRepo.AddAsync(chat);
                await _chatRepo.SaveChangesAsync();
            }
            Message newMessage = new Message
            {
                ChatId = chat.Id,
                Content = message.Content,
                SentAt = DateTime.UtcNow, // This will help keep a consistent time reference across different locations
                SenderId = currentUserId,
                ReceiverId = receiverId,
                IsSeen = false
            };
            await _messageRepo.AddAsync(newMessage);
            await _messageRepo.SaveChangesAsync();
            return await _responseService.GenerateSuccessResponseAsync("Message sent successfully");
        }
    }
}
