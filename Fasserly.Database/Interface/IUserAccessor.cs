using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Interface
{
    public interface IUserAccessor
    {
        public string GetCurrentUserName();
    }
}
