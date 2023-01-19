using Reenbit.HireMe.Domain.Entities;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<int> GetUserIdByEmail(string email);

        Task<User> GetUserByEmail(string email);

        Task<User> GetUserBId(int id);
    }
}
