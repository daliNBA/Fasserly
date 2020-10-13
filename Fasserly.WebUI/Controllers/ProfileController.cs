using Fasserly.Infrastructure.Profiles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    public class ProfileController : BaseController
    {
        [HttpGet("{username}")]
        public async Task<ActionResult<Profile>> Get(string username)
        {
            return await Mediator.Send(new Details.Query { Username = username });
        }
    }
}
