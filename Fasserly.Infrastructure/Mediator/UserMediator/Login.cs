using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public class Login
{
    public class Query : IRequest<UserFasserly>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidation : AbstractValidator<Query>
    {
        public LoginValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password est obligatoire");
        }
    }

    public class Handler : BaseDataAccess, IRequestHandler<Query, UserFasserly>
    {
        private readonly UserManager<UserFasserly> _userManager;
        private readonly SignInManager<UserFasserly> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public Handler(DbContextOptions<DatabaseContext> options, UserManager<UserFasserly> userManager, SignInManager<UserFasserly> signInManager, IJwtGenerator jwtGenerator) : base(options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<UserFasserly> Handle(Query request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized, new { training = "Not Authorized" });
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if(result.Succeeded)
            {
                return new UserFasserly
                {
                    UserName = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    DisplayName = user.DisplayName,
                    Image = null,
                };
            }
            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
