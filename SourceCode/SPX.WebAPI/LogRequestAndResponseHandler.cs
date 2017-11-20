using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using log4net;

namespace SPX.WebAPI
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        ILog _log = log4net.LogManager.GetLogger(typeof(LogRequestAndResponseHandler));
        private static log4net.ILog Log
        {
            get;
            set;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string url = request.RequestUri.AbsoluteUri;
            _log.Info(url);
            
            string requestBody = await request.Content.ReadAsStringAsync();
            _log.Info(requestBody);

            
            var result = await base.SendAsync(request, cancellationToken);

            if (result.Content != null)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                _log.Info(responseBody);
            }

            return result;
        }
    }
}