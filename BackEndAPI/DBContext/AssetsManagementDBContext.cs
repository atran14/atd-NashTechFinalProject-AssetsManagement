using System;
using BackEndAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.DBContext
{
    public class AssetsManagementDBContext : IdentityDbContext<User, Role, int>
    {
        public AssetsManagementDBContext(DbContextOptions<AssetsManagementDBContext> options)
             : base(options)
        {
        }

        
        
  

        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ReturnRequest> ReturnRequest { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AssetConfiguration).Assembly);

            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins").HasKey(x => new { x.UserId });

            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens").HasKey(x => new { x.UserId });

            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "Admin",
                Description = "Admin role"
            });

            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(
             new User
             {
                 Id = 1,
                 StaffCode = "SD0001",
                 FirstName = "Nguyen Van",
                 LastName = "Binh",
                 DateOfBirth = new DateTime(01 / 20 / 1993),
                 JoinedDate = new DateTime(12 / 05 / 2021),
                 Gender = Gender.Male,
                 Type = UserType.Admin,
                 UserName = "binhnv",
                 PasswordHash = hasher.HashPassword(null, "binhnv@20011993"),
                 Location = Location.HaNoi,
                 Status = UserStatus.Active,
                 NormalizedUserName = "Admin",
                 Email = null,
                 NormalizedEmail = null,
                 EmailConfirmed = true,
                 SecurityStamp = string.Empty
             });
             builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int> {
                 UserId = 1,
                 RoleId = 1
             });
        }
    }
}