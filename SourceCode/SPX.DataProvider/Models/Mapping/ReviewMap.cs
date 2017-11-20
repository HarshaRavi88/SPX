using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SPX.DataProvider.Models.Mapping
{
    public class ReviewMap : EntityTypeConfiguration<Review>
    {
        public ReviewMap()
        {
            // Primary Key
            this.HasKey(t => t.ReviewId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Reviews");
            this.Property(t => t.ReviewId).HasColumnName("ReviewId");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.User).HasColumnName("User");
            this.Property(t => t.Product_ProductId).HasColumnName("Product_ProductId");

            // Relationships
            this.HasOptional(t => t.Product)
                .WithMany(t => t.Reviews)
                .HasForeignKey(d => d.Product_ProductId);

        }
    }
}
