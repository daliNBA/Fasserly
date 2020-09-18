using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fasserly.Database.Entities
{
    public class Training
    {
        public Guid TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Rating { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Column(TypeName = "decimal(8,6)")]
        public decimal? Price { get; set; }

        public string Language { get; set; }
        public bool IsActive { get; set; }
        //public List<UserFasserly> UserFasserlies { get; set; } = new List<UserFasserly>();
        public Category category { get; set; }
        public List<Comment> comments { get; set; } = new List<Comment>();
    }
}
