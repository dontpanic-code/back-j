using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class MessagesRepository : EntityFrameworkRepository<Messages, int>, IMessagesRepository
    {
        //public async Task<Chats> GetChatById(int _id)
        //{
        //    return await this.DbSet.Where(c => c.Id == _id).Select(u => new Chats { Id = u.Id, DisplayName = u.DisplayName, TotalUnreadMessages = u.TotalUnreadMessages}).FirstOrDefaultAsync();
        //}

        //public async Task<int> GetUserIdByEmail(string email)
        //{
        //    return await this.DbSet.Where(c => c.CurrentEmail == email).Select(u => u.IdChat).FirstOrDefaultAsync();
        //}

        public async Task<List<Messages>> GetChatsById(int id)
        {
            return await this.DbSet.Where(c => c.ToId == id || c.FromId == id).ToListAsync();
        }
    }
}
