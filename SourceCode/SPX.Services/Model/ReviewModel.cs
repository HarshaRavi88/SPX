using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPX.Services.Model
{
    public class ReviewModel
    {
        public string ReviewId { get; set; }
        public string User { get; set; }
        public string Comments { get; set; }
        public string Rating { get; set; }
    }
}
