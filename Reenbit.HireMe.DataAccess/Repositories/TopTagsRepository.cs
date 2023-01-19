using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class TopTagsRepository : EntityFrameworkRepository<TopTags, int>, ITopTagsRepository
    {

        public Task<List<TopTags>> GetAllPosts()
        {
            IQueryable<TopTags> query = this.DbSet.Select(u => new TopTags { Tags = u.Tags, Count = u.Count }).OrderByDescending(u => u.Count);
            return query.ToListAsync();
        }

    }
}
