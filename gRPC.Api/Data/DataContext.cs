using gRPC.Api.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace gRPC.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().HasData(new UserEntity {Id = 1, Username = "admin", Password = "admin" });
        }
    }
}
