using AssecoPraksa.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssecoPraksa.Database.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public TransactionEntityTypeConfiguration() { }

        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.BeneficiaryName);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Direction).IsRequired().HasConversion<String>();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);
            builder.Property(x => x.Mcc).HasMaxLength(4);
            builder.Property(x => x.TransactionKind).IsRequired().HasConversion<String>();
            builder.Property(x => x.Catcode);

            builder.HasOne(t => t.Category).WithMany(c => c.Transactions).HasForeignKey(t => t.Catcode);
        }
    }
}
