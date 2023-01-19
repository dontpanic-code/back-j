using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class RecruiterRepository : EntityFrameworkRepository<Recruiter, int>, IRecruiterRepository
    {
        public void AddRecruiterBackup(RecruiterBackup candidateBackup)
        {
            this.DbContext.Set<RecruiterBackup>().Add(candidateBackup);
        }
        public async Task<Recruiter> ForUpdate(int idUser)
        {
            return await this.DbSet.Where(c => c.UserId == idUser).Select(u => new Recruiter { UserId = u.UserId, Id = u.Id, Company = u.Company, CompanyOther = u.CompanyOther, DateCreated = u.DateCreated, IsAnonymous = u.IsAnonymous, Position = u.Position, PositionOther = u.PositionOther}).FirstOrDefaultAsync();
        }
    }
}
