using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class ResendEmailVerification
    {
        public class Query : IRequest
        {
            public string Email { get; set; }
            public string Origin { get; set; }
        }
        public class Handler : IRequestHandler<Query>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IEMailSender _emailSender;

            public Handler(UserManager<UserFasserly> userManager, IEMailSender emailSender)
            {
                _userManager = userManager;
                _emailSender = emailSender;
            }
            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //ENcode the token with webEncoder
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var verifyUrl = $"{request.Origin}/user/verifyEmail?token={token}&email={request.Email}";
                var message = $"<p>Please click the below link to verify you email adress: <a href='{verifyUrl}'>{verifyUrl}<a/></p>";
                await _emailSender.SendEmailAsync(request.Email, "Verify adress", message);
                return Unit.Value;
            }
        }
    }
}
