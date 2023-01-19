using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class CandidatesRepository : EntityFrameworkRepository<Candidate, int>, ICandidatesRepository
    {
        public void AddCandidatesBackup(CandidateBackup candidateBackup)
        {
            this.DbContext.Set<CandidateBackup>().Add(candidateBackup);
        }

        public Task<List<Candidate>> GetApproved()
        {
            IQueryable<Candidate> query = this.DbSet.Where(c => c.IsApproved == true);
            return query.ToListAsync();
        }
        public Task<List<Candidate>> GetApprovedGuest()
        {
            IQueryable<Candidate> query = this.DbSet.Where(c => c.IsApproved == true).Select(u => new Candidate {Id = u.Id, Position = u.Position, City = u.City, ConsiderRelocation = u.ConsiderRelocation, Country = u.Country, CurrentLocation = u.CurrentLocation, EnglishLevel = u.EnglishLevel, EnglishSpeaking = u.EnglishSpeaking, ExperienceInYears = u.ExperienceInYears, IsRemote = u.IsRemote, LeadershipExperience = u.LeadershipExperience, Education = u.Education, Courses = u.Courses});
            return query.ToListAsync();
        }

        public Task<List<Candidate>> GetNotProcessed()
        {
            return this.DbSet.Where(c => c.IsApproved == null).ToListAsync();
        }

        private static string GetFilterString(string filterValue)
        {
            return filterValue.Trim().ToLower();
        }

        public Task<bool> IsContactRequestExists(int candidateId, int recruiterId)
        {
            return this.DbContext.Set<CandidateContact>().AnyAsync(cr => cr.CandidateId == candidateId && cr.RecruiterId == recruiterId);
        }

        public void AddCandidateContactRequest(CandidateContact candidateContact)
        {
            this.DbContext.Set<CandidateContact>().Add(candidateContact);
        }

        public async Task<Candidate> ForUpdate(int idUser)
        {
            return await this.DbSet.Where(c => c.UserId == idUser).Select(u => new Candidate { UserId = u.UserId, Id = u.Id, AllSelectedCompanies = u.AllSelectedCompanies, Position = u.Position, City = u.City, ConsiderRelocation = u.ConsiderRelocation, Country = u.Country, CurrentLocation = u.CurrentLocation, CvUrl = u.CvUrl, EnglishLevel = u.EnglishLevel, EnglishSpeaking = u.EnglishSpeaking, ExperienceInYears = u.ExperienceInYears, IsRemote = u.IsRemote, LeadershipExperience = u.LeadershipExperience, LinkedinUrl = u.LinkedinUrl, OwnNameCompany = u.OwnNameCompany, RejectionReason = u.RejectionReason, IsAnonymous = u.IsAnonymous, Education = u.Education, Courses = u.Courses}).FirstOrDefaultAsync();
        }
    }
}
