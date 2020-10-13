using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fasserly.Database.Entities
{
    public class UserFasserly : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public virtual ICollection<UserTraining> UserTrainings { get; set; } = new HashSet<UserTraining>();
        public virtual ICollection<Training> Trainings { get; set; } = new HashSet<Training>();
        public virtual ICollection<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
