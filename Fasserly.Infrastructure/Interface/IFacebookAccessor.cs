using Fasserly.Infrastructure.Mediator.UserMediator;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Interface
{
    public interface IFacebookAccessor
    {
        Task<FacebookUserInfo> FacebookLogin(string accessToken);
    }
}
