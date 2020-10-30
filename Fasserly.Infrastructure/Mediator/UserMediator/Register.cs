using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class Register
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
            public string DisplayName { get; set; }
            public string Origin { get; set; }
        }

        public class UserValidation : AbstractValidator<Command>
        {
            public UserValidation()
            {
                RuleFor(x => x.Email).EmailAddress();
                RuleFor(x => x.Password).Password();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IEMailSender _eMailSender;

            public Handler(DbContextOptions<DatabaseContext> options, UserManager<UserFasserly> userManager, IEMailSender eMailSender) : base(options)
            {
                _userManager = userManager;
                _eMailSender = eMailSender;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await context.Users.AnyAsync(x => x.Email == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email already exists" });
                if (await context.Users.AnyAsync(x => x.UserName == request.Username))
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });

                var user = new UserFasserly
                {
                    UserName = request.Username,
                    Email = request.Email,
                    DisplayName = request.DisplayName,
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                await _userManager.UpdateAsync(user);

                if (!result.Succeeded) throw new Exception("Problem creating user");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //ENcode the token with webEncoder
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var verifyUrl = $"{request.Origin}/user/verifyEmail?token={token}&email={request.Email}";
                var message = $"<p>Please click the below link to verify you email adress: <a href='{verifyUrl}'>{verifyUrl}<a/></p>";
                await _eMailSender.SendEmailAsync(request.Email, "Verify adress", message);
                return Unit.Value;
            }
        }
    }
}
