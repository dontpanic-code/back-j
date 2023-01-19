using Reenbit.HireMe.Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IMessagesRepository : IRepository<Messages, int>
    {
        //Task<int> GetUserIdByEmail(string email);
        //Task<Chats> GetChatById(int _id);

        Task<List<Messages>> GetChatsById(int id);
    }
}
