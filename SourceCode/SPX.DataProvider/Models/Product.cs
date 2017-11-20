using System;
using System.Collections.Generic;

namespace SPX.DataProvider.Models
{
    public class Product
    {
        public Product()
        {
            this.Reviews = new List<Review>();
        }

        public long ProductId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Brand { get; set; }
        public System.DateTime DatePublished { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
