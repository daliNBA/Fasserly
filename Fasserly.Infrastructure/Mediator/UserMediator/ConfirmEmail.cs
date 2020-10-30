using Fasserly.Database.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{

    public class ConfirmEmail
    {
        public class Command : IRequest<IdentityResult>
        {
            public string Token { get; set; }
            public string Email { get; set; }
        }

        public class CommandValidation : AbstractValidator<Command>
        {
            public CommandValidation()
            {
                RuleFor(x => x.Token).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is requuired");
            }
        }

        public class Handler : IRequestHandler<Command, IdentityResult>
        {
            private readonly UserManager<UserFasserly> _userManager;

            public Handler(UserManager<UserFasserly> userManager)
            {
                _userManager = userManager;
            }

            public async Task<IdentityResult> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                //here we decode the the token (opposite of WebEncoder of Register.cs register Handler)
                var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
                var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

                // identityCore will validate an email confirmation token is matches the specified user
                return await _userManager.ConfirmEmailAsync(user, decodedToken);
            }
        }
    }
}
