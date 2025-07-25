﻿using Microsoft.EntityFrameworkCore;
using tesstt.Models;

namespace tesstt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
    }
}
