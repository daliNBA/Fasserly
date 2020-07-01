using Fasserly.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fasserly.Database
{
    public class Seed
    {
        public static void SeedData(DatabaseContext context)
        {
            if (context.Trainings.Count() == 0)
            {
                var trainings = new List<Training>
                {
                    new Training
                    {
                        Title = "Scrum Master",
                        Description = "Apprendre pour devenir scrum master",
                        DateOfCreation = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Rating = 5,
                        Language = "Français",
                        IsActive = true
                    },
                    new Training
                    {
                        Title = "Scrum PO",
                        Description = "Apprendre pour devenir scrum master",
                        DateOfCreation = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Rating = 5,
                        Language = "Français",
                        IsActive = true
                    },
                    new Training
                    {
                        Title = "Scrum Developer",
                        Description = "Apprendre pour devenir scrum master",
                        DateOfCreation = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Rating = 5,
                        Language = "Français",
                        IsActive = true
                    }
                };
                context.Trainings.AddRange(trainings);
                context.SaveChanges();
            }
        }
    }
}
