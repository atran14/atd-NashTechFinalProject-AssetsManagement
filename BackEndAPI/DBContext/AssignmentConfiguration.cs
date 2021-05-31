using System;
using BackEndAPI.Entities;
using BackEndAPI.Enums;
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

        builder.HasOne(a=> a.Request)
                .WithOne(c => c.Assignment)
                .HasForeignKey<ReturnRequest>(c => c.AssignmentId)
                .OnDelete(deleteBehavior: DeleteBehavior.SetNull);

            builder.Ignore(e => e.AssignedToUser);

            builder.HasData(
                    new Assignment
                    {
                        Id = 1,
                        AssetId = 1,
                        AssignedByUserId = 1,
                        AssignedToUserId = 2,
                        AssignedDate = new DateTime(2021, 2, 15),
                        Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                        State = AssignmentState.Accepted
                    },
                    new Assignment
                    {
                        Id = 2,
                        AssetId = 2,
                        AssignedByUserId = 1,
                        AssignedToUserId = 3,
                        AssignedDate = new DateTime(2021, 2, 15),
                        Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                        State = AssignmentState.Accepted
                    },
                    new Assignment
                    {
                        Id = 3,
                        AssetId = 3,
                        AssignedByUserId = 4,
                        AssignedToUserId = 5,
                        AssignedDate = new DateTime(2021, 2, 15),
                        Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                        State = AssignmentState.WaitingForAcceptance
                    },
                    new Assignment
                    {
                        Id = 4,
                        AssetId = 4,
                        AssignedByUserId = 4,
                        AssignedToUserId = 6,
                        AssignedDate = new DateTime(2021, 2, 15),
                        Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                        State = AssignmentState.Accepted
                    }
            );
        }
    }
}