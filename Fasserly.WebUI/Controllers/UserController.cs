using Fasserly.Infrastructure.Mediator.UserMediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fasserly.WebUI.Controllers
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            var user = await Mediator.Send(query);
            SetRefreshToken(user.RefreshToken);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(Register.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            command.Origin = Request.Headers["origin"];
            await Mediator.Send(command);
            return Ok("registration successful - please check your email");
        }

        [AllowAnonymous]
        [HttpPost("verfiyEmail")]
        public async Task<ActionResult> VerfiyEmail(ConfirmEmail.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await Mediator.Send(command);
            if (!result.Succeeded) return BadRequest("Problem verifying email");

            return Ok("Verfication email successided - you can login!");
        }

        [AllowAnonymous]
        [HttpGet("resendEmailVerification")]
        public async Task<ActionResult> ResendEmailVerification([FromForm]ResendEmailVerification.Query query)
        {
            query.Origin = Request.Headers["origin"];
            await Mediator.Send(query);
            return Ok("Verfication email sent - please checkemail !");
        }

        [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            var user = await Mediator.Send(new CurrentUser.Query());
            SetRefreshToken(user.RefreshToken);
            return user;
        }

        [AllowAnonymous]
        [HttpPost("facebook")]
        public async Task<ActionResult<User>> Register(ExternalLogin.Query command)
        {
            var user = await Mediator.Send(command);
            SetRefreshToken(user.RefreshToken);
            return user;
        }

        [HttpPost("refreshToken")]
        public async Task<ActionResult<User>> RefreshToken(RefreshToken.Command command)
        {
            command.RefreshToken = Request.Cookies["refreshToken"];
            var user = await Mediator.Send(command);
            SetRefreshToken(user.RefreshToken);
            return user;
        }

        //Set refresh token to cookies
        private void SetRefreshToken(string refreshToken)
        {
            var cookiesOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookiesOption);
        }
    }
}
