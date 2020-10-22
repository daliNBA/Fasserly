using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.ProfileMediator
{
    public class ProfileReader : BaseDataAccess, IProfileReader
    {
        private readonly IUserAccessor _userAccessor;

        public ProfileReader(DbContextOptions<DatabaseContext> options, IUserAccessor userAccessor) : base(options)
        {
            _userAccessor = userAccessor;
        }

        public async Task<Profile> ReadProfile(string username)
        {
            var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (user == null)
                throw new RestException(HttpStatusCode.NotFound, new { user = "Not found" });
            var currentUser = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
            var profile = new Profile
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = user.Photos,
                Email = user.Email,
                Bio = user.Bio,
                FollowersCount = user.Followers.Count(),
                FollowingsCount = user.Followings.Count(),
            };
            if (currentUser != null)
                if (currentUser.Followings.Any(x => x.TargetId == user.Id))
                    profile.IsFollowed = true;

            return profile;
        }
    }
}
