using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPX.DataProvider.Models;
using SPX.Services.Model;
using SPX.Services.ViewModel;

namespace SPX.Services
{
    public interface IProductService
    {
        bool InsertProduct(Product payload);

        IEnumerable<ProductViewModel> GetAllProducts();

        bool UpdateProductReview(long reviewId, ReviewModel updatePayload);

        bool DeleteProductReview(long reviewId);

        List<ReviewViewModel> GetProductsReviewByTitle(string title);
    }
}
