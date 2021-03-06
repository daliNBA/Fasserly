﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Entities
{
    public class Promotion
    {
        public Guid PromotionId { get; set; }
        public int Percent { get; set; }
        public DateTime DateOfPromotion { get; set; }
        public DateTime DateOfEnd { get; set; }
        public virtual ICollection<Training> trainings { get; set; } = new HashSet<Training>();
    }
}
