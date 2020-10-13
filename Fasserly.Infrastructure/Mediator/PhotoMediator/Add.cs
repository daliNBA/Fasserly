using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.PhotoMediator
{

    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command, Photo>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IPhotoAccessor _photoAccessor;

            public Handler(DbContextOptions<DatabaseContext> options, IUserAccessor userAccessor, IPhotoAccessor photoAccessor) : base(options)
            {
                _userAccessor = userAccessor;
                _photoAccessor = photoAccessor;
            }

            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                var photoUploadResult = _photoAccessor.AddPhoto(request.File);
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.IsMain))
                    photo.IsMain = true;

                user.Photos.Add(photo);
                var success = await context.SaveChangesAsync() > 0;
                if (success) return photo;
                throw new Exception("Saving problem");
            }
        }
    }
}
