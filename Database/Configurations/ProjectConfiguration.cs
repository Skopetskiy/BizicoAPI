using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(o => o.Id);

            builder.Property(p => p.SkillLevel).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Brief).HasColumnType("varchar(1000)").IsRequired();
            builder.Property(p => p.Technology).HasColumnType("varchar(30)").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Date).HasColumnType("date").IsRequired();

            builder
               .HasOne(c => c.User)
               .WithMany(p => p.Projects)
               .HasForeignKey(c => c.UserId);
        }
    }
}
