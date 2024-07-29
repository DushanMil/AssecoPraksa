using AssecoPraksa.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AssecoPraksa.Database.Configurations
{
    public class TransactionSplitEntityTypeConfiguration : IEntityTypeConfiguration<TransactionSplitEntity>
    {
        public TransactionSplitEntityTypeConfiguration() { }

        public void Configure(EntityTypeBuilder<TransactionSplitEntity> builder)
        {
            builder.ToTable("transaction_splits");
            builder.HasKey(x => x.TransactionId);
            builder.HasKey(x => x.SplitId);
            builder.Property(x => x.Catcode);
            builder.Property(x => x.Amount).IsRequired();

            builder.HasOne(s => s.Category).WithMany(c => c.TransactionSplits).HasForeignKey(s => s.Catcode);
            builder.HasOne(s => s.Transaction).WithMany(t => t.TransactionSplits).HasForeignKey(s => s.TransactionId);
        }
    }
}
