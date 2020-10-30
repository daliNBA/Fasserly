using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.UserMediator
{
    public class ExternalLogin
    {
        public class Query : IRequest<User>
        {
            public string AccessToken { get; set; }
        }
        public class Handler : BaseDataAccess, IRequestHandler<Query, User>
        {
            private readonly UserManager<UserFasserly> _userManager;
            private readonly IFacebookAccessor _facebookAccessor;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(DbContextOptions<DatabaseContext> options, UserManager<UserFasserly> userManager, IFacebookAccessor facebookAccessor, IJwtGenerator jwtGenerator) : base(options)
            {
                _userManager = userManager;
                _facebookAccessor = facebookAccessor;
                _jwtGenerator = jwtGenerator;
            }
            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var userInfo = await _facebookAccessor.FacebookLogin(request.AccessToken);
                if (userInfo == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem validation token" });

                var user = await _userManager.FindByEmailAsync(userInfo.Email);
                var refreshToken = _jwtGenerator.GenerateRefreshToken();
                if (user != null)
                {
                    user.RefreshTokens.Add(refreshToken);
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);
                    return new User(user, _jwtGenerator, refreshToken.Token);
                }


                user = new UserFasserly
                {
                    DisplayName = userInfo.Name,
                    Id = userInfo.Id,
                    UserName = "fb_" + userInfo.Id,
                    Email = userInfo.Email,
                    EmailConfirmed = true,
                };

                var photo = new Photo
                {
                    Id = "fb_" + userInfo.Id,
                    Url = userInfo.Picture.Data.Url,
                    IsMain = true,
                };
                user.Photos.Add(photo);

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = "Problem validation token" });

                return new User(user, _jwtGenerator, refreshToken.Token);
            }
        }
    }
}
