using System;
using System.Collections.Generic;
using System.Text;

namespace Fasserly.Database.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Label { get; set; }
        public List<Training> Trainings { get; set; } = new List<Training>();
    }
}
