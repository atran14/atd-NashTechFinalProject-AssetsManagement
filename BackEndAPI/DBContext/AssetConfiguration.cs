using BackEndAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.AssetCode)
                    .IsRequired()
                    .HasMaxLength(30);

            builder.Property(e => e.AssetName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(e => e.CategoryId)
                    .IsRequired();

            builder.Property(e => e.State)
                    .IsRequired();

            builder.Property(e => e.Location)
                    .IsRequired();

            builder.HasOne(a => a.Category)
                    .WithMany(c => c.Assets);
        }
    }
}