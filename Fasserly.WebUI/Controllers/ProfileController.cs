using Fasserly.Infrastructure.Mediator.Followers;
using Fasserly.Infrastructure.Mediator.ProfileMediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpPost("{username}/follow")]
        public async Task<ActionResult<Unit>> Add(string username)
        {
            return await Mediator.Send(new Add.Command { Username = username });
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult<Unit>> Delete(string username)
        {
            return await Mediator.Send(new Delete.Command { Username = username });
        }

        [HttpGet("{username}/follow")]
        public async Task<ActionResult<List<Profile>>> GetProfiles(string username, string predicate)
        {
            return await Mediator.Send(new List.Query{ Username = username, Predicate = predicate });
        }
    }
}
