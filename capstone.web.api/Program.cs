using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using capstone.web.api.Data;


namespace capstone.web.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();

            // Retrieve the secret key from configuration
            var secretKey = builder.Configuration["JwtConfig:Secret"];
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);

            if (keyBytes.Length < 16)
            {
                throw new InvalidOperationException("The secret key must be at least 16 bytes long.");
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            // Add authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("GeneralAndAbove", policy => policy.RequireRole("Administrator", "General"));
                options.AddPolicy("ReadOnlyAndAbove", policy => policy.RequireRole("Administrator", "General", "ReadOnly"));
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer((builder.Configuration.GetConnectionString("DefaultConnection"))));

            // Enable authorization
            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("OpenCorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                //SeedDatabase(db);
                try
                {
                    DbInitializer.Initialize(db);  // Seed the database
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while seeding the database: " + ex.ToString());
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Registration System API V1");
                });
            }

           

            // Use CORS with the wide-open policy
            app.UseCors("OpenCorsPolicy");

            app.UseHttpsRedirection();

            // Ensure authentication and authorization middleware are added
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapUserEndpoints();
            app.MapControllers();

            //var summaries = new[]
            //{
            //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            //};

            //app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            //{
            //    var forecast = Enumerable.Range(1, 5).Select(index =>
            //        new WeatherForecast
            //        {
            //            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //            TemperatureC = Random.Shared.Next(-20, 55),
            //            Summary = summaries[Random.Shared.Next(summaries.Length)]
            //        })
            //        .ToArray();
            //    return forecast;
            //})
            //.WithName("GetWeatherForecast")
            //.WithOpenApi();

            app.Run();
        }
        static void SeedDatabase(AppDbContext context)
        {
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
        }
    }
}
