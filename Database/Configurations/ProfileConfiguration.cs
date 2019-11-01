using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");
            builder.HasKey(o => o.Id);

            builder.Property(p => p.FirstName).HasColumnType("nvarchar(30)").HasMaxLength(30);
            builder.Property(p => p.LastName).HasColumnType("nvarchar(30)").HasMaxLength(30);
            builder.Property(p => p.Summary).HasColumnType("nvarchar(1000)").HasMaxLength(1000);
            builder.Property(p => p.Experience).HasColumnType("integer");
        }
    }
}
