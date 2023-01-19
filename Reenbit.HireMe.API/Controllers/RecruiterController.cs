using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Infrastructure;
using Reenbit.HireMe.Services.Abstraction;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecruiterController : BaseController
    {
        private readonly IRecruiterService recruiterService;

        public RecruiterController(IRecruiterService RecruiterService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.recruiterService = RecruiterService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetApprovedRecruiter()
        //{
        //    var result = await this.RecruiterService.GetApprovedRecruiter();

        //    return Ok(result);
        //}

        [HttpGet]
        [Route("my")]
        [Authorize]
        public async Task<IActionResult> GetMyRecruiter()
        {
            var result = await this.recruiterService.GetRecruiterByUserId(this.UserEmail);

            return Ok(result);
        }


        [HttpGet]
        [Route("private")]
        [Authorize]
        public async Task<IActionResult> GetAllRecruiter()
        {
            if (!this.IsAdmin())
            {
                return Forbid();
            }

            var result = await this.recruiterService.GetRecruiterWithPrivateInfo();

            return Ok(result);
        }

        //[HttpGet]
        //[Route("notprocessed")]
        //[Authorize]
        //public async Task<IActionResult> GetNotProcessedRecruiter()
        //{
        //    if (!this.IsAdmin())
        //    {
        //        return Forbid();
        //    }

        //    var result = await this.RecruiterService.GetNotProcessedRecruiter();

        //    return Ok(result);
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRecruiter([FromBody]CreateRecruiterDTO candidateDTO)
        {
            await this.recruiterService.AddRecruiter(candidateDTO, this.UserEmail);

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteRecruiter()
        {
            await this.recruiterService.DeleteRecruiter(this.UserEmail);

            return Ok();
        }

        //[HttpPost]
        //[Route("{candidateId}/process")]
        //[Authorize]
        //public async Task<IActionResult> ProcessCandidate(int candidateId, [FromBody]ProcessCandidateDTO processCandidateDTO)
        //{
        //    if (!this.IsAdmin())
        //    {
        //        return Forbid();
        //    }

        //    await this.RecruiterService.ProcessCandidate(candidateId, processCandidateDTO);

        //    return Ok();
        //}

        //[HttpPost]
        //[Route("request-contacts")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RequestContacts([FromBody]RequestContactsDTO requestContactsDTO)
        //{
        //    var result = await this.RecruiterService.RequestCandidateContacts(requestContactsDTO, "igor.tabas@gmail.com");

        //    return Ok(new { succeed = result });
        //}
    }
}