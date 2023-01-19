using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class UserRepository : EntityFrameworkRepository<User, int>, IUserRepository
    {
        public async Task<int> GetUserIdByEmail(string email)
        {
            return await this.DbSet.Where(c => c.Email == email).Select(u => u.Id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await this.DbSet.Where(c => c.Email == email).Select(u => new User{Email = u.Email, FirstName = u.FirstName, FullName = u.FullName, Id = u.Id, LastName = u.LastName, TypeUser = u.TypeUser}).FirstOrDefaultAsync();
        }
        public async Task<User> GetUserBId(int id)
        {
            return await this.DbSet.Where(c => c.Id == id).Select(u => new User { Email = u.Email, FirstName = u.FirstName, FullName = u.FullName, Id = u.Id, LastName = u.LastName, TypeUser = u.TypeUser }).FirstOrDefaultAsync();
        }
    }
}
