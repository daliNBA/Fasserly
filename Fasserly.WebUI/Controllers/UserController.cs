using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Mediator.UserMediator;
using Fasserly.WebUI.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    public class UserController : BaseController
    {
        protected readonly UserDataServices dataService;
        public UserController(UserDataServices dataService)
        {
            this.dataService = dataService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserFasserly>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserFasserly>> Register(Register.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<UserFasserly>> CurrentUser()
        {
            return await dataService.GetCurrentUser();
        }
    }
}
