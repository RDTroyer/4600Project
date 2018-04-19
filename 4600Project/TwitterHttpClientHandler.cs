using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _4600Project
{
    public class TwitterHttpClientHandler : HttpClientHandler
    {
        private string _authorizationHeader;

        /// <summary>
        /// Constructor for TwitterHttpClientHandler. It is a child of the System.Net Class HttpClientHandler. 
        /// Makes sets the variable _authorizarionHeader equal to the passed authorizationHeader, as well as 
        /// turns off the use of cookies, and default credentials.
        ///  
        /// Preconditions: None
        /// Postconditions: 
        /// </summary>
        /// <param name="authorizationHeader"></param>
        public TwitterHttpClientHandler(string authorizationHeader)
        {
            UseCookies = false;
            UseDefaultCredentials = false;

            _authorizationHeader = authorizationHeader;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("User-Agent", "TwitterReader/1.0.0.0");
            request.Headers.ExpectContinue = false;
            request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            request.Headers.Add("Authorization", _authorizationHeader);
            request.Version = new Version("1.0");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/jpeg"));
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return base.SendAsync(request, cancellationToken);
        }
    }
}
