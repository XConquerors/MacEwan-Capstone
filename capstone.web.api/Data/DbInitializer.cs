using capstone.web.api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace capstone.web.api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            try
            {
                // Check if the Users table is empty
                if (!context.Users.Any())
                {
                    // Example seed users
                    context.Users.Add(new User
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com",
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin-password"),
                        Role = "Administrator"
                    });

                    context.Users.Add(new User
                    {
                        FirstName = "General",
                        LastName = "User",
                        Email = "general@example.com",
                        Username = "general",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("general-password"),
                        Role = "General"
                    });

                    context.SaveChanges();
                }

                // Check if the Categories table is empty
                if (!context.Categories.Any())
                {
                    var categories = new Category[]
                    {
                        new Category { CategoryName = "Tech-AI", Description = "Machine Learning" },
                        new Category { CategoryName = "Entertainment-TV Shows", Description = "Binge Series" },
                        new Category { CategoryName = "Tech-Cybersecurity", Description = "Data Protection" },
                        new Category { CategoryName = "Education-STEM", Description = "Science Technology" },
                        new Category { CategoryName = "Food & Beverage-Vegan", Description = "Plant-Based" },
                        new Category { CategoryName = "Personal Development-Mindfulness", Description = "Present Awareness" }
                    };

                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                    Console.WriteLine("Categories have been saved to the database.");
                }

                // Check if the Priorities table is empty
                if (!context.Priorities.Any())
                {
                    var priorities = new Priority[]
                    {
                        new Priority { PriorityName = "Critical", Description = "Immediate attention and resolution", PriorityLevel = 1 },
                        new Priority { PriorityName = "High", Description = "Very important but not urgent", PriorityLevel = 2 },
                        new Priority { PriorityName = "Medium", Description = "Moderately important and can be scheduled", PriorityLevel = 3 },
                        new Priority { PriorityName = "Low", Description = "Low urgency and can be addressed later", PriorityLevel = 4 },
                        new Priority { PriorityName = "Backlog", Description = "On hold and can be done later", PriorityLevel = 5 }
                    };

                    context.Priorities.AddRange(priorities);
                    context.SaveChanges();
                    Console.WriteLine("Priorities have been saved to the database.");
                }

                // Check if the ToDos table is empty
                if (!context.ToDos.Any())
                {
                    var todos = new ToDo[]
                    {
                        new ToDo
                        {
                            Name = "Implement Authentication",
                            Description = "Set up user registration and login functionality.",
                            CategoryId = context.Categories.First(c => c.CategoryName == "Tech-AI").CategoryId,
                            PriorityId = context.Priorities.First(p => p.PriorityName == "Critical").PriorityId
                        },
                        new ToDo
                        {
                            Name = "Create Marketing Plan",
                            Description = "Develop a comprehensive marketing strategy.",
                            CategoryId = context.Categories.First(c => c.CategoryName == "Personal Development-Mindfulness").CategoryId,
                            PriorityId = context.Priorities.First(p => p.PriorityName == "High").PriorityId
                        },
                        new ToDo
                        {
                            Name = "Design Database Schema",
                            Description = "Outline the database structure for the application.",
                            CategoryId = context.Categories.First(c => c.CategoryName == "Education-STEM").CategoryId,
                            PriorityId = context.Priorities.First(p => p.PriorityName == "Medium").PriorityId
                        }
                    };

                    context.ToDos.AddRange(todos);
                    context.SaveChanges();
                    Console.WriteLine("ToDos have been saved to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }
}
