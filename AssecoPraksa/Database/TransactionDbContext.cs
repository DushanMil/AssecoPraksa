using AssecoPraksa.Database.Configurations;
using AssecoPraksa.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssecoPraksa.Database
{
    public class TransactionDbContext: DbContext
    {
        // za sad gotovo
        public TransactionDbContext(DbContextOptions options) : base(options) { }

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TransactionSplitEntity> TransactionSplits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionSplitEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
