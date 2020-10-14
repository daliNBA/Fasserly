using Fasserly.Infrastructure.Mediator.ProfileMediator;
using MediatR;
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

        [HttpPut]
        public async Task<ActionResult<Unit>> Add(Edit.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}
