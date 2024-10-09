using capstone.web.api.Entities;
using Microsoft.EntityFrameworkCore;


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
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin-password"), // Securely hash passwords
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
                    new Category {  CategoryName = "Tech-AI", Description = "Machine Learning" },
                    new Category {  CategoryName = "Entertainment-TV Shows", Description = "Binge Series" },
                    new Category { CategoryName = "Tech-Cybersecurity", Description = "Data Protection" },
                    new Category { CategoryName = "Education-STEM", Description = "Science Technology" },
                    new Category { CategoryName = "Food & Beverage-Vegan", Description = "Plant-Based" },
                    new Category { CategoryName = "Personal Development-Mindfulness", Description = "Present Awareness" }
                    };

                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                    Console.WriteLine("Data has been saved to the database.");
                }
                if (!context.Priorities.Any())
                {
                    // Seeding data for Priorities
                    var priorities = new Priority[]
                    {
                        new Priority { PriorityName = "High", Description = "High importance", IsDeleted = false },
                        new Priority { PriorityName = "Medium", Description = "Medium importance", IsDeleted = false },
                        new Priority { PriorityName = "Low", Description = "Low importance", IsDeleted = false }
                    };

                    context.Priorities.AddRange(priorities);
                    context.SaveChanges();
                    Console.WriteLine("Priorities have been saved to the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
        }
    }

}
