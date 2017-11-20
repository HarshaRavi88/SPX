using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using SPX.Services;

namespace SPX.WebAPI.Filters
{
    public class ApiAuthFilter : GenericAuthenticationFilter
    {
          private readonly IUserService _userServices;

          public ApiAuthFilter()
        {
            _userServices = new UserService();
        }

        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        { 
            var userId = _userServices.Authenticate(username, password);
            if (userId > 0)
            {
                var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                    basicAuthenticationIdentity.UserId = userId;
                return true;
            }
            return false;
        }

       
    }
}