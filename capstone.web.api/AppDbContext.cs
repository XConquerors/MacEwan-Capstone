﻿using capstone.web.api;
using capstone.web.api.Entities;
using Microsoft.EntityFrameworkCore;

namespace capstone.web.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }
    }
}
