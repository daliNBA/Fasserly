using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class RefreshToken
    {
        public class Command : IRequest<User>
        {
            public string RefreshToken { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command, User>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;

            public Handler(DbContextOptions<DatabaseContext> options, UserManager<UserFasserly> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor) : base(options)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }

            public async Task<User> Handle(Command request, CancellationToken cancellationToken)
            {

                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUserName());
                var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefreshToken);
                if (oldToken != null && !oldToken.IsActive)
                    throw new RestException(HttpStatusCode.Unauthorized, new { Token = "Not Authorized" });
                if(oldToken != null)
                    oldToken.Revoked = DateTime.UtcNow;

                var newRefreshToken = _jwtGenerator.GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);

                await _userManager.UpdateAsync(user);
                return new User(user, _jwtGenerator, newRefreshToken.Token);
            }
        }
    }
}
