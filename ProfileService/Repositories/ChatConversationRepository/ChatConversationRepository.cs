using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Repositories.ChatConversationRepository
{
    public class ChatConversationRepository:IChatConversationRepository
    {

        private readonly LabourLinkProfileDbContext _context;
        public ChatConversationRepository(LabourLinkProfileDbContext labourLinkProfileDbContext)
        {
            _context = labourLinkProfileDbContext;
        }
        public async Task<Conversation> AddConversation(Conversation conversation)
        {
            var checkALreadyExit = await _context.Conversations.
                FirstOrDefaultAsync(x =>
                (x.User1Id == conversation.User1Id && x.User2Id == conversation.User2Id) ||
                (x.User2Id == conversation.User1Id && x.User1Id == conversation.User2Id));

            if (checkALreadyExit != null)
            {
                return null;
            }
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Conversation> GetConversation(Guid user1Id, Guid user2Id)
        {
            var checkConversation = await _context.Conversations.
               FirstOrDefaultAsync(x =>
               (x.User1Id == user1Id && x.User2Id == user2Id) ||
               (x.User2Id == user1Id && x.User1Id == user2Id));

            if (checkConversation == null)
            {
                return null;
            }
            return checkConversation;


        }

        public async Task<List<ConversationViewDto>> GetEmployerConversation(Guid userId)
        {



            var getEmplyerConversation = await _context.Conversations
                                       .Include(x => x.User2)
                                       .Where(x => x.User1Id == userId).Select(x => new ConversationViewDto
                                       {
                                           Id = x.Id,
                                           UserId = x.User2Id,
                                           FullName = x.User2.FullName,
                                           PhoneNumber = x.User2.PhoneNumber,
                                           ProfilePhotoUrl = x.User2.ProfilePhotoUrl,
                                           LastMessage = x.LastMessage,
                                           LastUpdated = x.LastUpdated,
                                           IsBlocked = x.IsBlocked,
                                           BlockedByUserId = x.BlockedByUserId,

                                       }
                                       ).OrderByDescending(x=>x.LastUpdated).ToListAsync();


            return getEmplyerConversation;



        }

        public async Task<List<ConversationViewDto>> GetLabourConversation(Guid userId)
        {
            var getLabourConversation = await _context.Conversations
                                                    .Include(x => x.User1)
                                                    .Where(x => x.User2Id == userId)
                                                    .Select(x => new ConversationViewDto
                                                    {
                                                        Id = x.Id,
                                                        UserId = x.User1Id,
                                                        FullName = x.User1.FullName,
                                                        PhoneNumber = x.User1.PhoneNumber,
                                                        ProfilePhotoUrl = null,
                                                        LastMessage = x.LastMessage,
                                                        LastUpdated = x.LastUpdated,
                                                        IsBlocked = x.IsBlocked,
                                                        BlockedByUserId = x.BlockedByUserId,

                                                    }).OrderByDescending(x=>x.LastUpdated).ToListAsync();
            return getLabourConversation;
        }

        public async Task<bool> UpdateConversation(Conversation conversation)
        {

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
