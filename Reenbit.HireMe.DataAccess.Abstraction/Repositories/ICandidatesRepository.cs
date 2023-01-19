using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface ICandidatesRepository : IRepository<Candidate, int>
    {
        void AddCandidatesBackup(CandidateBackup candidateBackup);

        Task<List<Candidate>> GetApproved();
        Task<List<Candidate>> GetApprovedGuest();

        Task<List<Candidate>> GetNotProcessed();

        Task<bool> IsContactRequestExists(int candidateId, int recruiterId);

        void AddCandidateContactRequest(CandidateContact candidateContact);

        Task<Candidate> ForUpdate(int idUser);
    }
}
