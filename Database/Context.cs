using System;
using System.Collections.Generic;
using System.Text;
using Database.Model;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class Context : DbContext
    {
        protected Context(string ConnectionString)
        {
            Database.EnsureCreated();
        }

        public DbSet<TgUser> TgUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=usersdata.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
