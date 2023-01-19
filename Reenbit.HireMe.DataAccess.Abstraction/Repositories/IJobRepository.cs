using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IJobRepository : IRepository<Job, int>
    {

        Task<List<Job>> GetJobsByEmail(string email);

        Task<Job> GetJobBId(int id);

        Task<List<Job>> GetApproved();

        Task<List<Job>> GetAllJobs();

        Task<string> GetJobEmailById(int id);
    }
}
