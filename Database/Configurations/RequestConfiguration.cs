using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");
            builder.HasKey(o => o.Id);

            builder.Property(p => p.SkillLevel).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Experience).HasColumnType("int").IsRequired();
            builder.Property(p => p.Technology).HasColumnType("varchar(30)").IsRequired().HasMaxLength(30);
            builder.Property(p => p.Date).HasColumnType("date").IsRequired();

            builder
               .HasOne(c => c.User)
               .WithMany(p => p.Requests)
               .HasForeignKey(c => c.UserId);
        }
    }
}
