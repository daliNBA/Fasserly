using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.PhotoMediator
{
    public class Delete
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAcessor;

            public Handler(DbContextOptions<DatabaseContext> options, IUserAccessor userAccessor, IPhotoAccessor photoAcessor) : base(options)
            {
                _userAccessor = userAccessor;
                _photoAcessor = photoAcessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                if (photo == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Photos = "Not found" });
                if (photo.IsMain)
                    throw new RestException(HttpStatusCode.NotFound, new { Photos = "You cannot delete your main photo" });
                var result = _photoAcessor.DeletePhoto(photo.Id);
                if (result == null)
                    throw new Exception("Problem deletin photo");

                user.Photos.Remove(photo);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving photo");
            }
        }
    }
}
