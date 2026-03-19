using TutorMatch.Models;

namespace TutorMatch.Interfaces
{
    public interface IConversationService
    {
        Task<List<Conversation>> GetAll();
        Task<List<Conversation?>> getByUserId(Guid id, string identifier);
        Task<Conversation> Create(Conversation conversation);

    }
}
