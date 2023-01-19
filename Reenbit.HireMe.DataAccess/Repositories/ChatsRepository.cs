using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class ChatsRepository : EntityFrameworkRepository<Chats, int>, IChatsRepository
    {
        public async Task<Chats> GetChatById(int _id)
        {
            return await this.DbSet.Where(c => c.Id == _id).Select(u => new Chats { Id = u.Id, DisplayName = u.DisplayName, TotalUnreadMessages = u.TotalUnreadMessages}).FirstOrDefaultAsync();
        }

        public async Task<int> GetUserIdByEmail(string email)
        {
            return await this.DbSet.Where(c => c.CurrentEmail == email).Select(u => u.IdChat).FirstOrDefaultAsync();
        }

        public async Task<List<Chats>> GetChatsById(int id) 
        {
            return await this.DbSet.Where(c => c.CurrentUserId == id || c.Id == id).ToListAsync();
        }

        public List<Chats> GetChatsByIdCopy(int id)
        {
            return this.DbSet.Where(c => c.CurrentUserId == id || c.Id == id).ToList();
        }

        public async Task<int> GetChatByEmailId(string email, int id)
        {
            return await this.DbSet.Where(c => c.CurrentEmail == email && c.Id == id).Select(u => u.IdChat).FirstOrDefaultAsync();
        }

        public async Task<Chats> ForUpdateUnread (int fromId, int toId)
        {
            return await this.DbSet.Where(c => c.CurrentUserId == fromId && c.Id == toId).Select(u => new Chats { IdChat = u.IdChat, TotalUnreadMessages = u.TotalUnreadMessages, CurrentUnread = u.CurrentUnread, CurrentEmail = u.CurrentEmail, CurrentName = u.CurrentName, CurrentUserId = u.CurrentUserId, DisplayName = u.DisplayName, Id = u.Id }).FirstOrDefaultAsync();
        }
    }
}
