using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reenbit.HireMe.Services.Abstraction
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllPosts();
        Task<List<Blog>> GetListLikesBookmarks();
        Task<Blog>OpenPost(int id);
        Task AddPost(Blog createJob);
        Task<List<Blog>> GetUserPosts(int id);
        Task DeletePost(int id);
        Task UpdateBookmarks(Blog createJob);
        Task UpdateLikes(Blog createJob);

        //Task<List<Blog>> GetPostsByUserEmail(string email);






        //Task<List<JobView>> GetApprovedJob();
        //Task AddJob(JobDTO createJob, string email);
        //Task<List<Job>> GetJobsByUserEmail(string email);
        //Task DeleteJob(int id);
        //Task UpdateJob(Job job);
        //Task<List<Job>> GetAllJob();
        //Task UpdateJobModerator(Job job);
    }
}
