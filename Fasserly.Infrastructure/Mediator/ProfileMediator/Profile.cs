using Fasserly.Database.Entities;
using System.Collections.Generic;

namespace Fasserly.Infrastructure.Mediator.ProfileMediator
{
    public class Profile
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
