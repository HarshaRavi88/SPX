using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPX.DataProvider;
using SPX.DataProvider.Models;
using SPX.Services.Model;
using SPX.Services.ViewModel;

namespace SPX.Services
{
    public class ProductService : IProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService()
        {
            _unitOfWork = new UnitOfWork();
        }


        public bool InsertProduct(Product payload)
        {
            var product = new Product()
            {
                Brand = payload.Brand,
                DatePublished = DateTime.Now,
                ShortDescription = payload.ShortDescription,
                Title = payload.Title,
                Reviews = GetReviews(payload.Reviews)

            };
            _unitOfWork.ProductTblRepository.Insert(product);
            _unitOfWork.Save();
            return true;
        }

        public IEnumerable<ProductViewModel> GetAllProducts()
        {
            var products = _unitOfWork.ProductTblRepository.GetAll().Where(p => p.ProductId > 0).ToList();

            var productsViewModel = products.Select(p => new ProductViewModel()
            {
                ProductId = p.ProductId,
                Brand = p.Brand,
                ShortDescription = p.ShortDescription,
                Title = p.Title,
                Reviews = GetAllProductReview(p.Reviews)
            });

            return productsViewModel;
        }

        public bool UpdateProductReview(long reviewId, ReviewModel updatePayload)
        {
            var reviewEntity = _unitOfWork.ReviewsTblRepository.Get().FirstOrDefault(r => r.ReviewId == reviewId);

            if (reviewEntity != null && reviewEntity.ReviewId > 0)
            {
                reviewEntity.User = updatePayload.User;
                reviewEntity.Rating = updatePayload.User;
                reviewEntity.Comments = updatePayload.Comments;

                _unitOfWork.ReviewsTblRepository.Update(reviewEntity);
                _unitOfWork.Save();
                return true;
            }

            return false;
        }

        public bool DeleteProductReview(long reviewId)
        {
            var reviewEntity = _unitOfWork.ReviewsTblRepository.Get().FirstOrDefault(r => r.ReviewId == reviewId);

            if (reviewEntity != null && reviewEntity.ReviewId > 0)
            {
                _unitOfWork.ReviewsTblRepository.Delete(reviewEntity);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        public List<ReviewViewModel> GetProductsReviewByTitle(string productTitle)
        {
            string query = productTitle.Trim().ToLower();
            var productIdListByTitle = _unitOfWork.ProductTblRepository.GetAll()
                    .Where(p => (p.Title.Trim().ToLower().Contains(query)))
                    .Select(p => p.ProductId).ToList();
            List<ReviewViewModel> reviewsByPId = null;
            if (productIdListByTitle.Any())
            {
                 reviewsByPId =
                    _unitOfWork.ReviewsTblRepository.Get()
                        .Where(r => r.Product_ProductId != null && productIdListByTitle.Contains((long)r.Product_ProductId)).Select(r =>
                            new ReviewViewModel()
                            {
                                User = r.User,
                                Rating = r.Rating,
                                Comments = r.Comments
                            })
                        .ToList();

            }

            return reviewsByPId;
        }

        private List<ReviewViewModel> GetAllProductReview(IEnumerable<Review> reviews)
        {
            return reviews.Select(review => new ReviewViewModel { User = review.User, Rating = review.Rating, Comments = review.Comments }).ToList();
        }

        private List<Review> GetReviews(IEnumerable<Review> reviews)
        {
            var reviewsList = new List<Review>();

            foreach (var review in reviews)
            {
                var rw = new Review { Comments = review.Comments, User = review.User, Rating = review.Rating };
                reviewsList.Add(review);

            }
            return reviewsList;


        }

    }
}
