using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction.Repositories
{
    public interface IRecruiterRepository : IRepository<Recruiter, int>
    {
        void AddRecruiterBackup(RecruiterBackup candidateBackup);

        Task<Recruiter> ForUpdate(int idUser); 
    }
}
