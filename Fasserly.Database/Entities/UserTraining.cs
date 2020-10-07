using System;

namespace Fasserly.Database.Entities
{
    public class UserTraining
    {
        public string UserFasserlyId { get; set; }
        public virtual UserFasserly UserFasserly { get; set; }
        public Guid TrainingId { get; set; }
        public virtual Training Training { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsOwner { get; set; }
    }
}
