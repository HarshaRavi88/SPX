using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SPX.DataProvider;
using SPX.DataProvider.Models;
using SPX.Services;
using SPX.Services.Model;
using SPX.WebAPI.Controllers;

namespace SPX.WebAPI.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private IProductService _productService;
        private IUnitOfWork _unitOfWork;
        private List<Product> _products;
        private List<Review> _reviews;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<Review> _reviewrepository;

        private HttpClient _client;
        private HttpResponseMessage _response;
        private const string ServiceBaseURL = "http://localhost:54028/";

        public void Initialize()
        {
            _productService = new ProductService();
            _unitOfWork = new UnitOfWork();

        }

        [TestMethod]
        public void GetAllProductsTest()
        {
            Initialize();

            var controller = new SpxController();

            controller.Request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(ServiceBaseURL + "api/SPX")
            };

            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = controller.Get();

            var responseResult =
                JsonConvert.DeserializeObject<List<Product>>(_response.Content.ReadAsStringAsync().Result);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);

            Assert.AreEqual(responseResult.Any(), true);

        }


        [TestMethod]
        public void GetReviewsByTitle()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/Get/products/Til/reviews")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = productController.GetReviews("titel");
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);

        }


        [TestMethod]
        public void GetReviewsByInvalidTitle()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/Get/products/prodtitle/reviews")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = productController.GetReviews("prodtitle");
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreNotEqual(_response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void GetAllReviews()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/Get/products/Til/reviews")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _response = productController.GetReviews("prodtitle");
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreNotEqual(_response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void InsertValidProductsTest()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/InsertProducts")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var product = new List<Product>()
            {
               new Product()
               {
                   Brand = "APV",
                   Title = "New Title",
                   ShortDescription = "Good Product",
                   DatePublished = DateTime.Now,
                   Reviews = new List<Review>()
                   {
                       new Review()
                       {
                           User = "HarshaRavi",
                           Rating = "4",
                           Comments = "Nice one!",
                       },
                        new Review()
                       {
                           User = "HarshaRavi",
                           Rating = "4",
                           Comments = "Nice one!",
                       }
                   }

               }
            };
            _response = productController.InsertProducts(product);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);

        }


        [TestMethod]
        public void InsertInValidProductsBrandTest()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/InsertProducts")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var product = new List<Product>()
            {
               new Product()
               {
                   Brand = "LGN",
                   Title = "New Title",
                   ShortDescription = "Good Product",
                   DatePublished = DateTime.Now,
                   Reviews = new List<Review>()
                   {
                       new Review()
                       {
                           User = "HarshaRavi",
                           Rating = "4",
                           Comments = "Nice one!",
                       },
                        new Review()
                       {
                           User = "HarshaRavi",
                           Rating = "4",
                           Comments = "Nice one!",
                       }
                   }

               }
            };
            _response = productController.InsertProducts(product);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void InsertInValidRatingTest()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/InsertProducts")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var product = new List<Product>()
            {
               new Product()
               {
                   Brand = "APV",
                   Title = "New Title",
                   ShortDescription = "Good Product",
                   DatePublished = DateTime.Now,
                   Reviews = new List<Review>()
                   {
                       new Review()
                       {
                           User = "HarshaRavi",
                           Rating = "6",
                           Comments = "Nice one!",
                       }
                   }

               }
            };
            _response = productController.InsertProducts(product);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreNotEqual(_response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void UpdateProductReview()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/1")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var reviewUpdate = new ReviewModel()
            {
                ReviewId = "2",
                User = "HarshaRavi",
                Rating = "5",
                Comments = "Good one!",
            };

            const int reviewId = 1;

            _response = productController.Put(reviewId, reviewUpdate);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void UpdateNotFoundProductReview()
        {
            Initialize();
            var productController = new SpxController()
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "api/SPX/121")
                }
            };
            productController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var reviewUpdate = new ReviewModel()
            {
                ReviewId = "2",
                User = "HarshaRavi",
                Rating = "5",
                Comments = "Good one!",
            };

            const int reviewId = 121;

            _response = productController.Put(reviewId, reviewUpdate);
            Assert.AreEqual(_response.StatusCode, HttpStatusCode.NotModified);
            Assert.AreNotEqual(_response.StatusCode, HttpStatusCode.NotFound);
        }

    }

}
