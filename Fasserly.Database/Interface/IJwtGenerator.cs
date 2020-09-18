using Fasserly.Database.Entities;

namespace Fasserly.Database.Interface
{
    public interface IJwtGenerator
    {
        string CreateToken(UserFasserly user);
    }
}
