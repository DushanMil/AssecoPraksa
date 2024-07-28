using AssecoPraksa.Database.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AssecoPraksa.Database.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public CategoryEntityTypeConfiguration() { }

        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(x => x.Code);
            builder.Property(x => x.ParentCode);
            builder.Property(x => x.Name);
        }
    }
}
