using System;

namespace Fasserly.Database.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public virtual UserFasserly UserFasserly { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow;
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Revoked { get; set; }
        public bool IsActive => Revoked == null & !IsExpired;
    }
}
