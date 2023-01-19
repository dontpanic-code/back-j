using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IJobViewRepository : IRepository<JobView, int>
    {
        Task<List<JobView>> GetApproved();
    }
}
