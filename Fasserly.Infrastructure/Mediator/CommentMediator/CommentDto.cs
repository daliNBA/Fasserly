using System;

namespace Fasserly.Infrastructure.Mediator.CommentMediator
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public string Body { get; set; }
        public string Username { get; set; }
        public string TrainingId { get; set; }
        public DateTime DateOfComment { get; set; }
        public string Image { get; set; }
        public string DisplayName { get; set; }
    }
}
