using BackEndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.DBContext
{
    public class AssetsManagementDBContext : DbContext
    {
        public AssetsManagementDBContext(DbContextOptions<AssetsManagementDBContext> options)
             : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ReturnRequest> ReturnRequest { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server = DESKTOP-BAIITTC\\SQLEXPRESS; Database = AssetManagement; Trusted_Connection=True; MultipleActiveResultSets = true");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AssetConfiguration).Assembly);
        }
    }
}