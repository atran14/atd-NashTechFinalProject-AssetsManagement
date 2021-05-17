using System;
using BackEndAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            builder.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(e => e.DateOfBirth)
                    .IsRequired();

            builder.Property(e => e.JoinedDate)
                    .IsRequired();

            builder.Property(e => e.Gender)
                    .IsRequired();

            builder.Property(e => e.Type)
                    .IsRequired();

            builder.Property(e => e.StaffCode)
                    .IsRequired();

            builder.Property(e => e.Location)
                    .IsRequired();

            builder.Property(e => e.Status)
                    .IsRequired();       
}
    }
}