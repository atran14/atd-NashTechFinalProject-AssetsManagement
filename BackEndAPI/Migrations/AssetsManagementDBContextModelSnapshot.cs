﻿// <auto-generated />
using System;
using BackEndAPI.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BackEndAPI.Migrations
{
    [DbContext(typeof(AssetsManagementDBContext))]
    partial class AssetsManagementDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BackEndAPI.Entities.Asset", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssetCode")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("AssetName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("InstalledDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<string>("Specification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Assets");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AssetCode = "LA000001",
                            AssetName = "Laptop 1",
                            CategoryId = 1,
                            InstalledDate = new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Location = 0,
                            Specification = "Balls-to-the-walls laptop, specced with the latest CPU and GPU",
                            State = 1
                        },
                        new
                        {
                            Id = 2,
                            AssetCode = "LA000002",
                            AssetName = "Laptop 2",
                            CategoryId = 1,
                            InstalledDate = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Location = 0,
                            Specification = "An even more balls-to-the-walls laptop, specced with even better CPU and GPU than laptop 1",
                            State = 0
                        },
                        new
                        {
                            Id = 3,
                            AssetCode = "PC000001",
                            AssetName = "PC 1",
                            CategoryId = 2,
                            InstalledDate = new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Location = 1,
                            Specification = "Balls-to-the-walls desktop, designed for ultimate Word experience",
                            State = 0
                        },
                        new
                        {
                            Id = 4,
                            AssetCode = "PC000002",
                            AssetName = "PC 2",
                            CategoryId = 2,
                            InstalledDate = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Location = 1,
                            Specification = "An even more balls-to-the-walls laptop, designed for the performant Excel workflow",
                            State = 1
                        });
                });

            modelBuilder.Entity("BackEndAPI.Entities.AssetCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("AssetCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryCode = "LA",
                            CategoryName = "Laptop"
                        },
                        new
                        {
                            Id = 2,
                            CategoryCode = "PC",
                            CategoryName = "PC"
                        });
                });

            modelBuilder.Entity("BackEndAPI.Entities.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssetId")
                        .HasColumnType("int");

                    b.Property<int>("AssignedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("AssignedToUserId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("AssignedByUserId");

                    b.ToTable("Assignments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AssetId = 1,
                            AssignedByUserId = 1,
                            AssignedDate = new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            AssignedToUserId = 2,
                            Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                            State = 1
                        },
                        new
                        {
                            Id = 2,
                            AssetId = 2,
                            AssignedByUserId = 1,
                            AssignedDate = new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            AssignedToUserId = 3,
                            Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                            State = 0
                        },
                        new
                        {
                            Id = 3,
                            AssetId = 3,
                            AssignedByUserId = 4,
                            AssignedDate = new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            AssignedToUserId = 5,
                            Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                            State = 0
                        },
                        new
                        {
                            Id = 4,
                            AssetId = 4,
                            AssignedByUserId = 4,
                            AssignedDate = new DateTime(2021, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            AssignedToUserId = 6,
                            Note = "Make sure to upgrade RAM when you have spare time. Thanks.",
                            State = 1
                        });
                });

            modelBuilder.Entity("BackEndAPI.Entities.ReturnRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AcceptedByUserId")
                        .HasColumnType("int");

                    b.Property<int>("AssignmentId")
                        .HasColumnType("int");

                    b.Property<int>("RequestedByUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ReturnedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AcceptedByUserId");

                    b.HasIndex("AssignmentId")
                        .IsUnique();

                    b.HasIndex("RequestedByUserId");

                    b.ToTable("ReturnRequest");
                });

            modelBuilder.Entity("BackEndAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BackEndAPI.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("OnFirstLogin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0d227e3c-9e17-4eeb-9c7a-0dab584151c1",
                            DateOfBirth = new DateTime(1993, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Binh",
                            Gender = 0,
                            JoinedDate = new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Van",
                            Location = 0,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "binhnv@20011993",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0001",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 0,
                            UserName = "binhnv"
                        },
                        new
                        {
                            Id = 2,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "e6cde009-fb2e-4513-83de-c5176f130408",
                            DateOfBirth = new DateTime(1994, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Binh",
                            Gender = 1,
                            JoinedDate = new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Thi",
                            Location = 0,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "binhnt@12011994",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0002",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 1,
                            UserName = "binhnt"
                        },
                        new
                        {
                            Id = 3,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "a840b416-cd27-4394-acc5-477f3c9b0f13",
                            DateOfBirth = new DateTime(1997, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Binh",
                            Gender = 1,
                            JoinedDate = new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Thi",
                            Location = 0,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "binhnt2@12011997",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0003",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 1,
                            UserName = "binhnt2"
                        },
                        new
                        {
                            Id = 4,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "22fc18d6-ae08-44e0-95dd-7a2f036064e1",
                            DateOfBirth = new DateTime(2000, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Anh",
                            Gender = 1,
                            JoinedDate = new DateTime(2018, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Duc",
                            Location = 1,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "anhnd@20012000",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0004",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 0,
                            UserName = "anhnd"
                        },
                        new
                        {
                            Id = 5,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "f21c5542-19e1-4e74-8c97-d3f6df314dc8",
                            DateOfBirth = new DateTime(1990, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Van",
                            Gender = 1,
                            JoinedDate = new DateTime(2021, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Thi",
                            Location = 1,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "binhnt@12011990",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0005",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 1,
                            UserName = "binhnt"
                        },
                        new
                        {
                            Id = 6,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "0f12f97b-8811-4d5a-8733-ef1237ed5993",
                            DateOfBirth = new DateTime(1987, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EmailConfirmed = false,
                            FirstName = "Binh",
                            Gender = 0,
                            JoinedDate = new DateTime(2019, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            LastName = "Nguyen Thi",
                            Location = 1,
                            LockoutEnabled = false,
                            OnFirstLogin = 0,
                            Password = "binhnt2@120187",
                            PhoneNumberConfirmed = false,
                            StaffCode = "SD0006",
                            Status = 0,
                            TwoFactorEnabled = false,
                            Type = 1,
                            UserName = "binhnt2"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("BackEndAPI.Entities.Asset", b =>
                {
                    b.HasOne("BackEndAPI.Entities.AssetCategory", "Category")
                        .WithMany("Assets")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BackEndAPI.Entities.Assignment", b =>
                {
                    b.HasOne("BackEndAPI.Entities.Asset", "Asset")
                        .WithMany("Assignments")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEndAPI.Entities.User", "AssignedByUser")
                        .WithMany("Assignments")
                        .HasForeignKey("AssignedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("AssignedByUser");
                });

            modelBuilder.Entity("BackEndAPI.Entities.ReturnRequest", b =>
                {
                    b.HasOne("BackEndAPI.Entities.User", "AcceptedByUser")
                        .WithMany()
                        .HasForeignKey("AcceptedByUserId");

                    b.HasOne("BackEndAPI.Entities.Assignment", "Assignment")
                        .WithOne("Request")
                        .HasForeignKey("BackEndAPI.Entities.ReturnRequest", "AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEndAPI.Entities.User", "RequestedByUser")
                        .WithMany("Requests")
                        .HasForeignKey("RequestedByUserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AcceptedByUser");

                    b.Navigation("Assignment");

                    b.Navigation("RequestedByUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("BackEndAPI.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BackEndAPI.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BackEndAPI.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("BackEndAPI.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEndAPI.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BackEndAPI.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BackEndAPI.Entities.Asset", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("BackEndAPI.Entities.AssetCategory", b =>
                {
                    b.Navigation("Assets");
                });

            modelBuilder.Entity("BackEndAPI.Entities.Assignment", b =>
                {
                    b.Navigation("Request");
                });

            modelBuilder.Entity("BackEndAPI.Entities.User", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}
