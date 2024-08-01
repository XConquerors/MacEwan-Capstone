﻿namespace capstone.web.api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using capstone.web.api.Data;

    public static class UserEndpoints
    {
        private static string GenerateJwtToken(User user, string secretKey)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // Important for role-based authorization
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var secretKey = "%^@#HD*@HD2387d223wyfi@67823gfSDHIFEQIWUC387f@3fhR$#@@jfwWEHI";

            endpoints.MapGet("/api/users", [Authorize(Policy = "ReadOnlyAndAbove")] async (AppDbContext db) =>
            {
                return Results.Ok(await db.Users.ToListAsync());
            });

            endpoints.MapGet("/api/users/{id}", [Authorize(Policy = "ReadOnlyAndAbove")] async (int id, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            endpoints.MapPost("/api/users", [Authorize(Policy = "AdministratorOnly")] async (User user, AppDbContext db) =>
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash); // Secure hashing
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.Id}", user);
            });

            endpoints.MapPut("/api/users/{id}", [Authorize(Policy = "AdministratorOnly")] async (int id, User updateUser, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is null) return Results.NotFound();

                user.FirstName = updateUser.FirstName;
                user.LastName = updateUser.LastName;
                user.Email = updateUser.Email;
                user.Username = updateUser.Username;
                user.Role = updateUser.Role;

                if (!string.IsNullOrWhiteSpace(updateUser.PasswordHash))
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateUser.PasswordHash);

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            endpoints.MapDelete("/api/users/{id}", [Authorize(Policy = "AdministratorOnly")] async (int id, AppDbContext db) =>
            {
                var user = await db.Users.FindAsync(id);
                if (user is not null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });

            endpoints.MapPost("/api/login", async (LoginDto login, AppDbContext db) =>
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Username == login.Username);
                if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
                    return Results.Unauthorized();

                var token = GenerateJwtToken(user, secretKey);
                return Results.Ok(new { Token = token });
            });

            endpoints.MapPost("/api/logout", [Authorize] () =>
            {
                // Clear token client-side by expiring or removing from storage
                return Results.Ok(new { Message = "Logout successful" });
            });
        }

        public class LoginDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

}
