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
    public class UsersController : BaseController
    {
        private readonly IUserService usersService;

        public UsersController(IUserService usersService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.usersService = usersService;
        }       

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCandidates()
        {
            await this.usersService.DeleteUser(this.UserEmail);

            return Ok();
        }
      
    }
}