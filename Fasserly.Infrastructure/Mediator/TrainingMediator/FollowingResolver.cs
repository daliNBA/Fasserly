using AutoMapper;
using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class FollowingResolver : BaseDataAccess, IValueResolver<UserTraining, BuyerDto, bool>
    {
        private readonly IUserAccessor _userAccessor;

        public FollowingResolver(DbContextOptions<DatabaseContext> databaseOptions, IUserAccessor userAccessor) : base(databaseOptions)
        {
            _userAccessor = userAccessor;
        }

        public bool Resolve(UserTraining source, BuyerDto destination, bool destMember, ResolutionContext _context)
        {
            var currentUser = context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName()).Result;
            if (currentUser.Followings.Any(x => x.TargetId == source.UserFasserlyId))
                return true;
            return false;
        }
    }
}
