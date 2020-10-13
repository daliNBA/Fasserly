using Fasserly.Infrastructure.Mediator.PhotoMediator;
using Microsoft.AspNetCore.Http;

namespace Fasserly.Infrastructure.Interface
{
    public interface IPhotoAccessor
    {
        PhotoUploadResult AddPhoto(IFormFile file);
        string DeletePhoto(string publicId);
    }
}
