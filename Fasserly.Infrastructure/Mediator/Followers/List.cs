using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Mediator.ProfileMediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.Followers
{
    public class List
    {

        public class Query : IRequest<List<Profile>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }
        public class Handler : BaseDataAccess, IRequestHandler<Query, List<Profile>>
        {
            private readonly IProfileReader _profileReader;

            public Handler(DbContextOptions<DatabaseContext> options, IProfileReader profileReader) : base(options)
            {
                _profileReader = profileReader;
            }
            public async Task<List<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.UserFollowings.AsQueryable();
                var userFollowings = new List<UserFollowing>();
                var profiles = new List<Profile>();

                switch (request.Predicate)
                {
                    case "followers":
                        {
                            userFollowings = await query.Where(x => x.Target.UserName == request.Username).ToListAsync();
                            foreach (var follower in userFollowings)
                            {
                                profiles.Add(await _profileReader.ReadProfile(follower.Observer.UserName));
                            }
                            break;
                        }
                    case "followings":
                        {
                            userFollowings = await query.Where(x => x.Observer.UserName == request.Username).ToListAsync();
                            foreach (var follower in userFollowings)
                            {
                                profiles.Add(await _profileReader.ReadProfile(follower.Target.UserName));
                            }
                            break;
                        }
                }
                return profiles;
            }
        }
    }
}
