﻿using System;
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
    public class ChatsController : BaseController
    {
        private readonly IChatsService candidatesService;

        public ChatsController(IChatsService candidatesService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.candidatesService = candidatesService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetApprovedCandidates()
        {
            var result = await this.candidatesService.GetCandidatesWithPrivateInfo(this.UserEmail);

            if (result != null)
                return Ok(result);
            else
                return Ok();
        }

        //[HttpGet]
        //[Route("my")]
        //[Authorize]
        //public async Task<IActionResult> GetMyCandidate()
        //{
        //    var result = await this.candidatesService.GetCandidateByUserId(this.UserEmail);

        //    return Ok(result);
        //}


        //[HttpGet]
        //[Route("private")]
        //[Authorize]
        //public async Task<IActionResult> GetAllCandidates()
        //{
        //    if (!this.IsAdmin())
        //    {
        //        return Forbid();
        //    }

        //    var result = await this.candidatesService.GetCandidatesWithPrivateInfo();

        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("notprocessed")]
        //[Authorize]
        //public async Task<IActionResult> GetNotProcessedCandidates()
        //{
        //    if (!this.IsAdmin())
        //    {
        //        return Forbid();
        //    }

        //    var result = await this.candidatesService.GetNotProcessedCandidates();

        //    return Ok(result);
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCandidates([FromBody]CreateChatsDTO candidateDTO)
        {
            await this.candidatesService.AddCandidate(candidateDTO);

            return Ok();
        }

        //[HttpDelete]
        //[Authorize]
        //public async Task<IActionResult> DeleteCandidates()
        //{
        //    await this.candidatesService.DeleteCandidate(this.UserEmail);

        //    return Ok();
        //}

        //[HttpPost]
        //[Route("{candidateId}/process")]
        //[Authorize]
        //public async Task<IActionResult> ProcessCandidate(int candidateId, [FromBody]ProcessCandidateDTO processCandidateDTO)
        //{
        //    if (!this.IsAdmin())
        //    {
        //        return Forbid();
        //    }

        //    await this.candidatesService.ProcessCandidate(candidateId, processCandidateDTO);

        //    return Ok();
        //}

        //[HttpPost]
        //[Route("request-contacts")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RequestContacts([FromBody]RequestContactsDTO requestContactsDTO)
        //{
        //    var result = await this.candidatesService.RequestCandidateContacts(requestContactsDTO, "igor.tabas@gmail.com");

        //    return Ok(new { succeed = result });
        //}
    }
}