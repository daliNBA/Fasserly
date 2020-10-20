using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Body { get; set; }
        public virtual UserFasserly Author { get; set; }
        public virtual Training Training { get; set; }
        public DateTime DateOfComment { get; set; }
    }
}
