using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class JobViewRepository : EntityFrameworkRepository<JobView, int>, IJobViewRepository
    {
        public Task<List<JobView>> GetApproved()
        {
            IQueryable<JobView> query = this.DbSet.Where(c => c.IsApproved == true);
            return query.ToListAsync();
        }
    }
}
