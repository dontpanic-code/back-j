using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reenbit.HireMe.API.Extensions;
using Reenbit.HireMe.API.Models;
using Reenbit.HireMe.DataAccess.Abstraction;
using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using Reenbit.HireMe.Domain.DTOs;
using Reenbit.HireMe.Domain.Entities;
using Reenbit.HireMe.Infrastructure;
using Reenbit.HireMe.Services.Abstraction;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private string userType;
        //private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IUserService userService;
        public AuthController(IUserService userService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromQuery] LoginModel loginModel)
        {
            // Note: the "provider" parameter corresponds to the external
            // authentication provider choosen by the user agent.           
            if (string.IsNullOrWhiteSpace(loginModel.Provider))
            {
                return BadRequest();
            }

            if (!await HttpContext.IsProviderSupportedAsync(loginModel.Provider))
            {
                return BadRequest();
            }

            // Instruct the middleware corresponding to the requested external identity
            // provider to redirect the user agent to its own authorization endpoint.
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            this.userType = loginModel.TypeUser; 
            return Challenge(new AuthenticationProperties { RedirectUri = "/api/auth/aftersignin?redirectUrl=" + loginModel.RedirectUrl + "&&typeUser=" + loginModel.TypeUser, IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) }, loginModel.Provider );
        }

        [HttpGet]
        [Route("aftersignin")]
        public IActionResult AfterSignIn([FromQuery] string redirectUrl, [FromQuery] string typeUser)
        {
            this.userType = typeUser;
            this.userService.Add(this.GetUser());
            if(string.IsNullOrWhiteSpace(redirectUrl))
            {
                redirectUrl = "/";
            }
            return Redirect(redirectUrl);
        }

        
        [HttpGet]
        [Route("getProfile")]
        [Authorize]
        public IActionResult GetProfile()
        {
            Task<User> a = this.userService.GetUser(this.GetUser());
            string jsonString = JsonSerializer.Serialize<Task<User>>(a);
            return Ok(jsonString);
            //return Ok(this.userService.GetUser(this.GetUser()));
            //return Ok(this.GetUser());
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public IActionResult LogOut()
        {
           return this.SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private UserDTO GetUser()
        {
            return new UserDTO
            {
                FullName = HttpContext.GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"),
                Email = HttpContext.GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"),                
                FirstName = HttpContext.GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"),
                LastName = HttpContext.GetClaimValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"),
                TypeUser = this.userType,
                IsAdmin = this.IsAdmin()
            };
        }
    }
}