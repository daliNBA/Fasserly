using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class CurrentUser
    {
        public class Query : IRequest<User> { }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;

            public Handler(UserManager<UserFasserly> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
            }
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUserName());
                return new User
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    DisplayName = user.DisplayName,
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                };
            }
        }
    }
}
