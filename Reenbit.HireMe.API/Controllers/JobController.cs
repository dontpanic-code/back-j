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
    public class JobController : BaseController
    {
        private readonly IJobService jobService;

        public JobController(IJobService jobService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.jobService = jobService;
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetApprovedCandidates()
        {
            string key = "01234567890123456789012345678901"; // 32 bytes key, corresponds to AES-256
            string plaintext = "The quick brown fox jumps over the lazy dog";
            string encrypted = EncryptString(key, plaintext);

            object result = await this.jobService.GetApprovedJob();
            string combinedString = string.Join(",", result);

            encrypted = EncryptString(key, combinedString);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobDTO jobDTO)
        {
            await this.jobService.AddJob(jobDTO, this.UserEmail);
            return Ok();
        }

        [HttpGet]
        [Route("myjobs")]
        public async Task<IActionResult> GetMyJobs()
        {
            var result = await this.jobService.GetJobsByUserEmail(this.UserEmail);
            return Ok(result);
        }

        [HttpPost]
        [Route("deletejob")]
        public async Task<IActionResult> DeleteJob([FromBody] int id)
        {
            await this.jobService.DeleteJob(id);
            return Ok();
        }

        [HttpPost]
        [Route("udpadtejob")]
        public async Task<IActionResult> UpdateJob([FromBody] Job jobDTO)
        {
            await this.jobService.UpdateJob(jobDTO);
            return Ok();
        }

        [HttpGet]
        [Route("alljobs")]
        public async Task<IActionResult> AllJob()
        {
            var result = await this.jobService.GetAllJob();
            return Ok(result);
        }

        [HttpPost]
        [Route("updatejobmod")]
        public async Task<IActionResult> UpdateJobModerator([FromBody] Job jobDTO)
        {
            await this.jobService.UpdateJobModerator(jobDTO);
            return Ok();

        }
    }
}