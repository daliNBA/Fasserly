using Fasserly.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Infrastructure.Profiles
{
    public class Profile
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
