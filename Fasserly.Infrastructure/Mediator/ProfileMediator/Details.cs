using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.ProfileMediator
{
    public class Details
    {
        public class Query : IRequest<Profile>
        {
            public string Username { get; set; }
        }
        public class Handler : BaseDataAccess, IRequestHandler<Query, Profile>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }
            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);
                return new Profile
                {
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                    Photos = user.Photos,
                    Email =user.Email,
                    Bio = user.Bio
                };
            }
        }
    }
}
