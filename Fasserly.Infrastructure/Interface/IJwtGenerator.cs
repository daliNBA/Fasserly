using Fasserly.Database.Entities;

namespace Fasserly.Infrastructure.Interface
{
    public interface IJwtGenerator
    {
        string CreateToken(UserFasserly user);
        RefreshToken GenerateRefreshToken();
    }
}
