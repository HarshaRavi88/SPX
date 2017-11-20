using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPX.DataProvider.Models
{
    public class Review
    {
        public long ReviewId { get; set; }
       
        public string Rating { get; set; }
     
        public string Comments { get; set; }
        public string User { get; set; }
        public long? Product_ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
