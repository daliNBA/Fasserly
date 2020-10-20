using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Mediator.CategoryMediator;
using Fasserly.Infrastructure.Mediator.CommentMediator;
using Fasserly.Infrastructure.Mediator.UserMediator;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class TrainingDto
    {
        public Guid TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Rating { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? Price { get; set; }
        public string Language { get; set; }
        public bool IsActive { get; set; }
        public CategoryDto Category { get; set; }

        [JsonPropertyName("buyers")]
        public ICollection<BuyerDto> UserTrainings { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
