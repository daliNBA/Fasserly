using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fasserly.Database.Entities
{
    public class UserFasserly : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
