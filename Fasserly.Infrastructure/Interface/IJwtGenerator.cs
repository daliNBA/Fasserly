using Fasserly.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Infrastructure.Interface
{
    public interface IJwtGenerator
    {
        string CreateToken(UserFasserly user);
    }
}
