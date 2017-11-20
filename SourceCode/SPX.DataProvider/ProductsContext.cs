using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPX.DataProvider.Models
{
    public class ProductsContext : DbContext
    {
        static ProductsContext()
        {
            //Database.SetInitializer<ProductsContext>(null);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductsContext, SPX.DataProvider.Migrations.Configuration>("ProductsContext"));
           
        }

        //public ProductsContext()
        //    : base("Name=ProductsContext")
        //{
           
        //}

        public DbSet<Products> Products { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Review> Review { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        
       


    }
}
