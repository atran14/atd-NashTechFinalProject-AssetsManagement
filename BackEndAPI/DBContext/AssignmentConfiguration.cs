using BackEndAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.AssetId)
                    .IsRequired();

            builder.Property(e => e.AssignedByUserId)
                    .IsRequired();

            builder.Property(e => e.AssignedToUserId)
                    .IsRequired();

            builder.Property(e => e.AssignedDate)
                    .IsRequired();

            builder.Property(e => e.State)
                    .IsRequired();
                    
            builder.HasOne(a => a.Asset)
                    .WithMany(c => c.Assignments);
                    
            builder.HasOne(a => a.AssignedByUser)
                    .WithMany(c => c.Assignments);

            builder.Ignore(e=>e.AssignedToUser);
        }
    }
}