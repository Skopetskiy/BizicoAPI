using Database.Configurations;
using Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Database
{
    public class FreelanceContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Request> Requests { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Profile> Profiles { get; set; }
       

        public FreelanceContext()
        {}

        public FreelanceContext(DbContextOptions<FreelanceContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
