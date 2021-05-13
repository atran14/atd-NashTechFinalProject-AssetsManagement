using System;
using BackEndAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEndAPI.DBContext
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

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

            builder.Property(e => e.Username)
                    .IsRequired();

            builder.Property(e => e.Password)
                    .IsRequired();

            builder.Property(e => e.Location)
                    .IsRequired();

            builder.Property(e => e.Status)
                    .IsRequired();
                    
            builder.HasData(
                    new User
                    {
                        Id = 1,
                        StaffCode = "SD0001",
                        FirstName = "Binh",
                        LastName = "Nguyen Van",
                        DateOfBirth = new DateTime(1993,01,20),
                        JoinedDate = new DateTime(2021,12,05),
                        Gender = Gender.Male,
                        Type = UserType.Admin,
                        Username = "binhnv",
                        Password = "binhnv@20011993",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    },
                    new User
                    {
                        Id = 2,
                        StaffCode = "SD0002",
                        FirstName = "Binh",
                        LastName = "Nguyen Thi",
                        DateOfBirth = new DateTime(1994,01,12).Date,
                        JoinedDate = new DateTime(2021,12,05).Date,
                        Gender = Gender.Female,
                        Type = UserType.User,
                        Username = "binhnt",
                        Password = "binhnt@12011994",
                        Location = Location.HaNoi,
                        Status = UserStatus.Active
                    }
            );
        }
    }
}