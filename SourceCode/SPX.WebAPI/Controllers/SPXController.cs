using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Microsoft.AspNet.Identity;
using SPX.DataProvider.Models;
using SPX.Services;
using SPX.Services.Model;
using SPX.WebAPI.Filters;

namespace SPX.WebAPI.Controllers
{
    public class SpxController : ApiController
    {
        ILog _log = log4net.LogManager.GetLogger(typeof(SpxController));

        private IProductService _productService;

        public SpxController()
        {
            _productService = new ProductService();
        }
        // GET: api/SPX

        public HttpResponseMessage Get()
        {
            try
            {
                var products = _productService.GetAllProducts();

                return Request.CreateResponse(HttpStatusCode.OK, products, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.Forbidden,
                    new StatusResponse()
                    {
                        Response = string.Format("ERROR IN API REQUEST CALL. PLEASE CONTACT ADMINISTRATOR."),
                        Status = "ERROR"
                    },
                    new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                throw;

            }
            finally
            {
                _productService = null;
            }

        }

        // POST: api/SPX
        public HttpResponseMessage InsertProducts([FromBody] List<Product> payload)
        {
            try
            {
                bool apiError = false;

                if (payload == null || payload.Count <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new StatusResponse()
                    {
                        Response = "REQUEST CONTENT IS NULL, JSON PAYLOAD IS MOST LIKELY IN BAD FORMAT.",
                        Status = Helper.Constants.Error
                    },
                        new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }
                else
                {
                    HttpResponseMessage response;
                    if (PayloadValidateionRules(payload, out response)) return response;

                    foreach (var product in payload)
                    {
                        bool isProductSaved = _productService.InsertProduct(product);

                        if (!isProductSaved)
                        {
                            return Request.CreateResponse(HttpStatusCode.Forbidden, new StatusResponse()
                            {
                                Response = "FAILED TO COMPLETE THE REQUEST PROCESS",
                                Status = Helper.Constants.Error
                            }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, new StatusResponse()
                        {
                            Response = "REQUEST PROCESS SUCCESSFULLY AND PRODUCT INSERTED",
                            Status = Helper.Constants.Success
                        }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                    }

                }

            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.Forbidden,
                    new StatusResponse()
                    {
                        Response = string.Format("ERROR IN API REQUEST CALL. PLEASE CONTACT ADMINISTRATOR."),
                        Status = "ERROR"
                    },
                    new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                throw;

            }
            finally
            {
                _productService = null;
            }
            return null;


        }

        // PUT: api/SPX/5
        [System.Web.Http.HttpPut]
        public HttpResponseMessage Put(int id, [FromBody]ReviewModel updatePayload)
        {
            try
            {
                if (id > 0 || id != null)
                {
                    if (updatePayload != null)
                    {
                        HttpResponseMessage response;
                        if (ReviewPayloadValidateion(updatePayload, out response)) return response;

                        var isUpdated = _productService.UpdateProductReview(id, updatePayload);
                        return isUpdated ? Request.CreateResponse(HttpStatusCode.OK, new StatusResponse() { Response = "UPADATE REQUEST IS SUCCESS", Status = "SUCCESS" }, new System.Net.Http.Formatting.JsonMediaTypeFormatter())
                            : Request.CreateResponse(HttpStatusCode.NotModified, new StatusResponse() { Response = "NO REVIEW TO UPADATE", Status = "NOT FOUND" }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.Forbidden, new StatusResponse() { Response = "UPADATE REQUEST HAS FAILED", Status = "FAILED" }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                throw;
            }
            finally
            {
                _productService = null;
            }
            return null;
        }

        // DELETE: api/SPX/5
        [System.Web.Http.HttpDelete]
        [ApiAuthFilter]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id > 0 || id != null)
                {
                    var isDeleted = _productService.DeleteProductReview(id);
                    return isDeleted
                        ? Request.CreateResponse(HttpStatusCode.OK,
                            new StatusResponse() { Response = "REVIEW DELETED SUCCESSFULLY", Status = "SUCCESS" },
                            new System.Net.Http.Formatting.JsonMediaTypeFormatter())
                        : Request.CreateResponse(HttpStatusCode.NotFound,
                            new StatusResponse() { Response = "REVIEW NOT FOUND TO DELETE", Status = "NOT FOUND" },
                            new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, new StatusResponse() { Response = "DELETE REQUEST HAS FAILED", Status = "FAILED" }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                throw;
            }
            return null;
        }

        // GET: api/SPX/Get/products/{productTitle}/reviews
        [Route("api/SPX/Get/products/{productTitle}/reviews")]
        [HttpGet]
        public HttpResponseMessage GetReviews(string productTitle)
        {
            try
            {
                var reviews = _productService.GetProductsReviewByTitle(productTitle);

                if (reviews == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        new StatusResponse() { Status = "NOT FOUND", Response = "REQUESTED TITLE NOT FOUND" },
                        new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }
                if (reviews.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, reviews,
                        new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.Forbidden,
                new StatusResponse() { Status = "ERROR", Response = "REQUEST FAILED" },
                new System.Net.Http.Formatting.JsonMediaTypeFormatter());
                throw;
            }
            return null;
        }

        private bool PayloadValidateionRules(IEnumerable<Product> payload, out HttpResponseMessage response)
        {
            #region BRAND TYPE VALIDATION
            var isBrandNull = payload.Any(p => p.Brand == "" || string.IsNullOrWhiteSpace(p.Brand));

            if (isBrandNull)
            {
                return GetBadRequestResponse("BRAND TYPE IS NULL AND REQUIRED IN REQUEST PAYLOAD.", out response);
            }

            if (!isBrandNull)
            {
                var brandList = payload.Select(r => r.Brand.TrimStart().TrimEnd()).ToList();
                brandList = brandList.ConvertAll(l => l.ToUpper());
                var desiredItems = new List<string> { Helper.Constants.APV, Helper.Constants.Airpel, Helper.Constants.PowerTeam };
                desiredItems = desiredItems.ConvertAll(l => l.ToUpper());
                if (brandList.Except(desiredItems).Any())
                {
                    return GetBadRequestResponse("BRAND TYPE IS INVALID IN REQUEST PAYLOAD.", out response);
                }
            }

            #endregion

            #region RATING VALIDATION

            var reviewsList = payload.Select(r => r.Reviews).ToList();
            var reviewsRatingList = (from items in reviewsList from r in items select r.Rating).ToList();
            if (reviewsRatingList.Any(string.IsNullOrWhiteSpace))
            {
                return GetBadRequestResponse("RATING IS NULL AND REQUIRED IN REQUEST PAYLOAD.", out response);
            }

            if (reviewsRatingList.Any(rating => Convert.ToInt32(rating) <= 0 || Convert.ToInt32(rating) > 5))
            {
                return GetBadRequestResponse("RATING VALUE SHOULD BE BETWEEN 1 AND 5 IN REQUEST PAYLOAD.", out response);
            }

            #endregion

            #region RATING VALIDATION
            var commentList = payload.Select(r => r.Reviews).ToList();

            if (commentList.Any(comment => comment.Any(c => c.Comments.Length > 256)))
            {
                return GetBadRequestResponse("COMMENTS SHOULD BE LESS THAN 256 CHARACTERS IN REQUEST PAYLOAD.", out response);
            }
            #endregion
            response = null;
            return false;
        }

        private bool GetBadRequestResponse(string errorMessage, out HttpResponseMessage response)
        {

            response = Request.CreateResponse(HttpStatusCode.BadRequest,
                new StatusResponse()
                {
                    Response = errorMessage,
                    Status = Helper.Constants.Error
                }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            return true;
        }

        private bool ReviewPayloadValidateion(ReviewModel updatePayload, out HttpResponseMessage response)
        {
            #region RATING VALIDATION

            if (string.IsNullOrWhiteSpace(updatePayload.Rating))
            {
                return GetBadRequestResponse("RATING IS NULL AND REQUIRED IN REQUEST PAYLOAD.", out response);
            }

            if (!string.IsNullOrWhiteSpace(updatePayload.Rating))
            {
                if (Convert.ToInt32(updatePayload.Rating) <= 0 || Convert.ToInt32(updatePayload.Rating) > 5)
                {
                    return GetBadRequestResponse("RATING VALUE SHOULD BE BETWEEN 1 AND 5 IN REQUEST PAYLOAD.", out response);
                }
            }

            #endregion

            #region RATING VALIDATION

            if (updatePayload.Comments.Length > 256)
            {
                return GetBadRequestResponse("COMMENTS SHOULD BE LESS THAN 256 CHARACTERS IN REQUEST PAYLOAD.", out response);
            }
            #endregion

            response = null;
            return false;
        }


    }
}
