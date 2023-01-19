using Microsoft.EntityFrameworkCore;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Repositories
{
    public class JobRepository : EntityFrameworkRepository<Job, int>, IJobRepository
    {

        public Task<List<Job>> GetJobsByEmail(string email)
        {
            IQueryable<Job> query = this.DbSet.Where(c => c.Email == email);
            return query.ToListAsync();
        }
        public async Task<Job> GetJobBId(int id)
        {
            return await this.DbSet.Where(c => c.Id == id).Select(u => new Job { Id = u.Id, Email = u.Email, AboutProject = u.AboutProject, Benefits = u.Benefits, CompanyName = u.CompanyName, Contacts = u.Contacts, EmploymentType = u.EmploymentType, EnglishLevel = u.EnglishLevel, JobRequirements = u.JobRequirements, JobTitle = u.JobTitle, SalaryRange = u.SalaryRange, Stack = u.Stack, StagesInterview = u.StagesInterview, WorkplaceType = u.WorkplaceType, IsApproved = u.IsApproved, Country = u.Country, City = u.City, Experience = u.Experience }).FirstOrDefaultAsync();
        }

        public Task<List<Job>> GetApproved()
        {
            IQueryable<Job> query = this.DbSet.Where(c => c.IsApproved == true);
            return query.ToListAsync();
        }
        public Task<List<Job>> GetAllJobs()
        {
            IQueryable<Job> query = this.DbSet.Where(c => c.Id >= 1);
            return query.ToListAsync();
        }

        public async Task<string> GetJobEmailById(int id)
        {
            return await this.DbSet.Where(c => c.Id == id).Select(u => u.Email).FirstOrDefaultAsync();
        }
    }
}
