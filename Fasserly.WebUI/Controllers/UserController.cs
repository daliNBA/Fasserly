using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.WebUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        protected readonly UserDataServices dataService;
        public UserController(UserDataServices dataService)
        {
            this.dataService = dataService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserFasserly>> Login(string mail, string password)
        {
            return await dataService.GetUserByEmail(mail, password);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserFasserly>> Register(UserViewModel viewModel)
        {
            return await dataService.Registre(ParserToUserFasserly(viewModel));
        }

        [HttpGet]
        public async Task<ActionResult<UserFasserly>> CurrentUser()
        {
            return await dataService.GetCurrentUser();
        }

        private UserFasserly ParserToUserFasserly(UserViewModel vm)
        {
            return new UserFasserly
            {
                UserName = vm.UserName,
                DisplayName = vm.DisplayName,
                Email = vm.Email,
                Image = null,
            };
        }
    }
}
