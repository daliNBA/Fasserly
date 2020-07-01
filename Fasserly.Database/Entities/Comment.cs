﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Text { get; set; }
        public UserFasserly UserFasserly { get; set; }
        public DateTime DateOfComment { get; set; }
        public Training Training { get; set; }
    }
}