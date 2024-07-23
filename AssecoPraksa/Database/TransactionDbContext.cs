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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}
