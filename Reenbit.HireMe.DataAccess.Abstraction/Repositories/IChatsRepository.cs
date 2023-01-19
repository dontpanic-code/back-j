using Reenbit.HireMe.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IChatsRepository : IRepository<Chats, int>
    {
        Task<int> GetUserIdByEmail(string email);

        Task<Chats> GetChatById(int _id);

        Task<List<Chats>> GetChatsById(int id);

        List<Chats> GetChatsByIdCopy(int id);

        Task<int> GetChatByEmailId(string email, int id);

        Task<Chats> ForUpdateUnread(int fromId, int toId);
    }
}
