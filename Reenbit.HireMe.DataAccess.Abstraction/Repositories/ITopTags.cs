using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface ITopTagsRepository : IRepository<TopTags, int>
    {

        Task<List<TopTags>> GetAllPosts();
    }
}
