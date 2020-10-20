using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.ProfileMediator
{
    public interface IProfileReader
    {
        Task<Profile> ReadProfile(string username);
    }
}
