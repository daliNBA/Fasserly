using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.PhotoMediator
{

    public class SetMain
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
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
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photos = "Not found" });

                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                currentMain.IsMain = false;
                photo.IsMain = true;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }
    }
}
