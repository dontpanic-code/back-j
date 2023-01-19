using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Infrastructure;
using Reenbit.HireMe.Services.Abstraction;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : BaseController
    {
        private readonly IBlogService blogService;

        public BlogController(IBlogService blogService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllPosts()
        {
            object result = await this.blogService.GetAllPosts();
            return Ok(result);
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetListLikesBookmarks()
        {
            object result = await this.blogService.GetListLikesBookmarks();
            return Ok(result);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> AddPost([FromBody] Blog blog)
        {
            await this.blogService.AddPost(blog);
            return Ok();
        }

        [HttpPost]
        [Route("openpost")]
        public async Task<IActionResult> OpenPost([FromBody] int id)
        {
            object result =  await this.blogService.OpenPost(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("userposts")]
        public async Task<IActionResult> GetUserPost([FromBody] int id)
        {
            object result = await this.blogService.GetUserPosts(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteJob([FromBody] int id)
        {
            await this.blogService.DeletePost(id);
            return Ok();
        }

        [HttpPost]
        [Route("updateBookmarks")]
        public async Task<IActionResult> UpdateBookmarks([FromBody] Blog jobDTO)
        {
            await this.blogService.UpdateBookmarks(jobDTO);
            return Ok();

        }

        [HttpPost]
        [Route("updateLikes")]
        public async Task<IActionResult> UpdateLikes([FromBody] Blog jobDTO)
        {
            await this.blogService.UpdateLikes(jobDTO);
            return Ok();

        }

        //[HttpPost]
        //public async Task<IActionResult> CreateJob([FromBody] JobDTO jobDTO)
        //{
        //    await this.jobService.AddJob(jobDTO, this.UserEmail);
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("myjobs")]
        //public async Task<IActionResult> GetMyJobs()
        //{
        //    var result = await this.jobService.GetJobsByUserEmail(this.UserEmail);
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("deletejob")]
        //public async Task<IActionResult> DeleteJob([FromBody] int id)
        //{
        //    await this.jobService.DeleteJob(id);
        //    return Ok();
        //}

        //[HttpPost]
        //[Route("udpadtejob")]
        //public async Task<IActionResult> UpdateJob([FromBody] Job jobDTO)
        //{
        //    await this.jobService.UpdateJob(jobDTO);
        //    return Ok();
        //}

        //[HttpGet]
        //[Route("alljobs")]
        //public async Task<IActionResult> AllJob()
        //{
        //    var result = await this.jobService.GetAllJob();
        //    return Ok(result);
        //}

        //[HttpPost]
        //[Route("updatejobmod")]
        //public async Task<IActionResult> UpdateJobModerator([FromBody] Job jobDTO)
        //{
        //    await this.jobService.UpdateJobModerator(jobDTO);
        //    return Ok();

        //}
    }
}