using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Reenbit.HireMe.Infrastructure;
using Reenbit.HireMe.Services.Abstraction;

namespace Reenbit.HireMe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopTagsController : BaseController
    {
        private readonly ITopTagsService topTagsService;

        public TopTagsController(ITopTagsService topTagsService, IConfigurationManager configurationManager)
            : base(configurationManager)
        {
            this.topTagsService = topTagsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            object result = await this.topTagsService.GetAllPosts();
            return Ok(result);
        }
    }
}