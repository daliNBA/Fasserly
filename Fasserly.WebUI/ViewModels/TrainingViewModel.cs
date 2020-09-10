using Fasserly.Database.Entities;
using System;

namespace Fasserly.WebUI.ViewModels
{
    public class TrainingViewModel
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

        public TrainingViewModel()
        {
        }

        public TrainingViewModel(Training training)
        {
            TrainingId = training.TrainingId;
            Title = training.Title;
            Description = training.Description;
            Rating = training.Rating;
            DateOfCreation = training.DateOfCreation;
            UpdateDate = training.UpdateDate;
            Price = training.Price;
            Language = training.Language;
            IsActive = training.IsActive;
        }
    }
}
