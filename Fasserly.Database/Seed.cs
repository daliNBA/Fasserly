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
            if (context.Categories.Count() == 0)
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                       Label="Web"
                    }, new Category
                    {
                      Label= "Game"
                    }, new Category
                    {
                      Label= "Mobil"
                    }, new Category
                    {
                      Label= "Graphic Conception"
                    }, new Category
                    {
                      Label= "Animation"
                    }, new Category
                    {
                      Label= "Management"
                    }
                };

                foreach (var cat in categories)
                {
                    await context.AddAsync(cat);
                    context.SaveChanges();
                }
            }
            if (!userManager.Users.Any())
            {
                var users = new List<UserFasserly>
                {
                    new UserFasserly
                    {
                        Id ="a",
                        DisplayName ="Dali",
                        Email = "dali.mahdoui@gmail.com",
                        UserName = "dalinba"
                    },
                      new UserFasserly
                    {
                        Id="b",
                        DisplayName ="Rachida",
                        Email = "rachidachtioui13@yahoo.fr",
                        UserName = "rachida"
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$W0rd");
                }

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
                        IsActive = true,
                        category = new Category
                        {
                            Label ="Management",
                        },
                        UserTrainings = new List<UserTraining>
                        {
                            new UserTraining
                            {
                                UserFasserlyId = "a",
                                IsOwner = true,
                                DateJoined =DateTime.Now.AddMonths(-2)
                            },
                            new UserTraining
                            {
                                UserFasserlyId = "b",
                                IsOwner = false,
                                DateJoined =DateTime.Now.AddMonths(-9)
                            }
                         }
                    },
                    new Training
                    {
                        Title = "Product owner",
                        Description = "Apprendre pour devenir scrum master",
                        DateOfCreation = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Rating = 5,
                        Language = "Français",
                        IsActive = true,
                         category = new Category
                        {
                            Label ="Management",
                        },
                        UserTrainings = new List<UserTraining>
                        {
                            new UserTraining
                            {
                                UserFasserlyId = "a",
                                IsOwner = false,
                                DateJoined =DateTime.Now.AddMonths(-2)
                            },
                            new UserTraining
                            {
                                UserFasserlyId = "b",
                                IsOwner = true,
                                DateJoined =DateTime.Now.AddMonths(-9)
                            }
                         }
                    },
                    new Training
                    {
                        Title = "Programmation",
                        Description = "Apprendre pour devenir scrum master",
                        DateOfCreation = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Rating = 5,
                        Language = "Français",
                        IsActive = true,
                        category = new Category
                        {
                            Label ="Web",
                        },
                        UserTrainings = new List<UserTraining>
                        {
                            new UserTraining
                            {
                                UserFasserlyId = "a",
                                IsOwner = false,
                                DateJoined =DateTime.Now.AddMonths(-2)
                            },
                            new UserTraining
                            {
                                UserFasserlyId = "b",
                                IsOwner = true,
                                DateJoined =DateTime.Now.AddMonths(-9)
                            }
                         }
                    }
                };
                    context.Trainings.AddRange(trainings);
                    context.SaveChanges();
                }
            }
        }
    }
}
