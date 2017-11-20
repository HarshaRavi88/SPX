using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SPX.DataProvider.Models;
using SPX.WebAPI.Models;

namespace SPX.WebAPI.Filters
{
    public class GenericAuthenticationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var identity = FetchAuthHeader(filterContext);
            if (identity == null)
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            if (!OnAuthorizeUser(identity.Name, identity.Password, filterContext))
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
          
            base.OnAuthorization(filterContext);
        }

        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;
            if (authRequest != null && !String.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
                authHeaderValue = authRequest.Parameter;
            if (string.IsNullOrEmpty(authHeaderValue))
                return null;
            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            var credentials = authHeaderValue.Split(':');
            return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], credentials[1]);
        }

        private static void ChallengeAuthRequest(HttpActionContext filterContext)
        {
           // var dnsHost = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new StatusResponse()
            {
                Response = "UNAUTHORIZED ACCESS TO THE API",
                Status = "ACCESS_DENIED"
            }, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
          //  filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));

        }

        protected virtual bool OnAuthorizeUser(string user, string pass, HttpActionContext filterContext)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                return false;
            return true;
        }

    }
}