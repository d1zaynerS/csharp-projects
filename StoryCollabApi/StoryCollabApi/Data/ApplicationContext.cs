using Microsoft.EntityFrameworkCore;
using StoryCollabApi.Models;
using System.Reflection.Emit;

namespace StoryCollabApi.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<StoryUser> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StoryUser>().HasData(
                new StoryUser { Id = 1, Name = "Max", Email = "max@gmail.com" },
                new StoryUser { Id = 2, Name = "Tom", Email = "tom@example.com" },
                new StoryUser { Id = 3, Name = "Anna", Email = "anna@example.com" }
            );
        }
    }
}