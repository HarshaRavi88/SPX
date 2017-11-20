using System.Collections.Generic;

namespace SPX.Services.ViewModel
{
   public class ProductViewModel
    {
        public long ProductId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Brand { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }
}
