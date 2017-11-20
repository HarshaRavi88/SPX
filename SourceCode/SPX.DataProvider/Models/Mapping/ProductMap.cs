using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SPX.DataProvider.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.ProductId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Products");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.DatePublished).HasColumnName("DatePublished");
        }
    }
}
