using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IJobService
    {
        Task<List<JobView>> GetApprovedJob();
        Task AddJob(JobDTO createJob, string email);
        Task<List<Job>> GetJobsByUserEmail(string email);
        Task DeleteJob(int id);
        Task UpdateJob(Job job);
        Task<List<Job>> GetAllJob();
        Task UpdateJobModerator(Job job);
    }
}
