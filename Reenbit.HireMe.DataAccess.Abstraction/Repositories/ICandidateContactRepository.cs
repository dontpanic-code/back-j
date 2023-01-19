using Reenbit.HireMe.Domain.Entities;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface ICandidateContactRepository : IRepository<CandidateContact, int>
    {
        Task<bool> IsRequestAlreadyMade(int recruiterId, int candidateId);
    }
}
