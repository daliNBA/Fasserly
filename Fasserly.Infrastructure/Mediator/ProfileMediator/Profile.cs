using Fasserly.Database.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fasserly.Infrastructure.Mediator.ProfileMediator
{
    public class Profile
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string Bio { get; set; }

        [JsonPropertyName("following")]
        public bool IsFollowed { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
        public string Email { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
