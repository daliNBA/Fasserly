using Fasserly.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fasserly.Database
{
    public class Seed
    {
        public static async Task SeedData(DatabaseContext context, UserManager<UserFasserly> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<UserFasserly>
                {
                    new UserFasserly
                    {
                        DisplayName ="Dali",
                        Email = "dali.mahdoui@gmail.com",
                        UserName = "dalinba"
                    },
                      new UserFasserly
                    {
                        DisplayName ="Rachida",
                        Email = "rachidachtioui13@yahoo.fr",
                        UserName = "rachida"
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "pa$$W0rd");
                }
            }

            if (context.Trainings.Count() == 2)
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
