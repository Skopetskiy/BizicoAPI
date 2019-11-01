using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(o => o.Id);

            builder
               .HasMany(c => c.Requests)
               .WithOne(p => p.User)
               .HasForeignKey(c => c.UserId);
            builder
              .HasMany(c => c.Projects)
              .WithOne(p => p.User)
              .HasForeignKey(c => c.UserId);

            builder
                .HasOne(c => c.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(c => c.UserId);

            builder
                .HasMany(c => c.UserRoles)
                .WithOne(p => p.User)
                .HasForeignKey(c => c.UserId);
        }
    }
}
