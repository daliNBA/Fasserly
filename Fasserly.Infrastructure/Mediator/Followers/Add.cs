using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.Followers
{

    public class Add
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            private readonly IUserAccessor _userAccessor;

            public Handler(DbContextOptions<DatabaseContext> options, IUserAccessor userAccessor) : base(options)
            {
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
                var target = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);
                if (target == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });
                if (!target.UserTrainings.Any(x => x.IsOwner))
                    throw new RestException(HttpStatusCode.NotFound, new { observer = "This account is not a trainer" });
 
                var following = await context.UserFollowings.SingleOrDefaultAsync(x => x.ObserverId == observer.Id && x.TargetId == target.Id);

                if (following != null)
                    throw new RestException(HttpStatusCode.NotFound, new { following = "Following all ready exist" });

               if(following == null)
                {
                    following = new UserFollowing
                    {
                        Observer = observer,
                        Target = target,
                    };
                    context.Add(following);
                }

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }
    }
}
