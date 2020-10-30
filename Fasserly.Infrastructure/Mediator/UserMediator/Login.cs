using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using Fasserly.Infrastructure.Mediator.UserMediator;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public class Login
{
    public class Query : IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Image { get; set; }
    }

    public class LoginValidation : AbstractValidator<Query>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password est obligatoire");
        }
    }

    public class Handler : IRequestHandler<Query, User>
    {
        private readonly UserManager<UserFasserly> _userManager;
        private readonly SignInManager<UserFasserly> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public Handler(UserManager<UserFasserly> userManager, SignInManager<UserFasserly> signInManager, IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<User> Handle(Query request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new { training = "Not Authorized" });
            if (!user.EmailConfirmed)
                throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email is not confirmed" });
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            var refreshToken = _jwtGenerator.GenerateRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);


            if (result.Succeeded)
                return new User(user, _jwtGenerator, refreshToken.Token);

            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
