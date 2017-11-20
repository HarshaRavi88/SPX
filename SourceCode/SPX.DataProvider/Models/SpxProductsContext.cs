using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SPX.DataProvider.Models.Mapping;

namespace SPX.DataProvider.Models
{
    public partial class SpxProductsContext : DbContext
    {
        static SpxProductsContext()
        {
            Database.SetInitializer<SpxProductsContext>(new CreateDatabaseIfNotExists<SpxProductsContext>());
        }

        public SpxProductsContext()
            : base("Name=SpxProductsContext")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ReviewMap());
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
