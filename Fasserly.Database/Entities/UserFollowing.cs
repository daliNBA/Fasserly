using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Entities
{
    public class UserFollowing
    {
        public string ObserverId { get; set; }
        public virtual UserFasserly Observer { get; set; }
        public string TargetId { get; set; }
        public virtual UserFasserly Target { get; set; }
    }
}
