using System;
using System.Collections.Generic;

namespace Fasserly.Database.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Label { get; set; }
        public virtual ICollection<Training> Trainings { get; set; } = new HashSet<Training>();
    }
}
