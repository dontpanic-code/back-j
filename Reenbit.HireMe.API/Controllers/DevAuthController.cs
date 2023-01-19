using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevAuthController : ControllerBase
    {
        [HttpGet]
        [Route("devlog")]
        public async Task Authenticate(string pwd)
        {
            if (pwd == "stayhome")
            {
                var claims = new List<Claim>
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "DEV"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "DEV"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "DEV"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname", "DEV"),
                };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            }
        }
    }
}