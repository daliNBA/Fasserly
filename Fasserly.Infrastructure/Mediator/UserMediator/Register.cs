using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class Register
    {
        public class Command : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public string Displayname { get; set; }
        }

        public class UserValidation : AbstractValidator<Command>
        {
            public UserValidation()
            {
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).Password();
                RuleFor(x => x.Username).NotEmpty();
                //RuleFor(x => x.Displayname).NotEmpty();
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command, User>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DbContextOptions<DatabaseContext> options, UserManager<UserFasserly> userManager, IJwtGenerator jwtGenerator) : base(options)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.AnyAsync(x => x.Email == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });
                if (await context.Users.AnyAsync(x => x.UserName == request.Username))
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });

                var user = new UserFasserly
                {
                    UserName = request.Username,
                    Email = request.Email,
                    DisplayName = request.Displayname,
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    return new User
                    {
                        DisplayName=user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        Image = null,
                        Username = user.UserName,
                    };
                }

                throw new Exception("Problem saving changes");
            }
        }
    }
}
