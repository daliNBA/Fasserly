using MediatR;
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
        public class Handler : IRequestHandler<Query, Profile>
        {
            private readonly IProfileReader _profileReader;

            public Handler(IProfileReader profileReader)
            {
                _profileReader = profileReader;
            }
            public async Task<Profile> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _profileReader.ReadProfile(request.Username);
            }
        }
    }
}
