using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CategoryExhibitorConfiguration : IEntityTypeConfiguration<CategoryExhibitor>
    {
        public void Configure(EntityTypeBuilder<CategoryExhibitor> builder)
        {
            builder.ToTable("CategoryExhibitor");
            builder
                .HasKey(ce => new {ce.CategoryId, ce.ExhibitorId});

            builder
                .HasOne(ce => ce.Category)
                .WithMany("CategoryExhibitors");
            builder
                .HasOne(ce => ce.Exhibitor)
                .WithMany("CategoryExhibitors");
        }
    }
}