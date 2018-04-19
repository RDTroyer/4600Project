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
        /// Postconditions: sets _authorizationHeader to the passed value.
        /// </summary>
        /// <param name="authorizationHeader">Passed Authorization Header to be added.</param>
        public TwitterHttpClientHandler(string authorizationHeader)
        {
            UseCookies = false;
            UseDefaultCredentials = false;

            _authorizationHeader = authorizationHeader;
        }

        /// <summary>
        /// This function overrides HttpClientHandler's SendAsync function, Which adds the TwitterImageViewer
        /// to a collection of Headers in the HttpClientHandler. It then sets ExpectContinue to false, telling
        /// the Header that there is no need to use the expect-continue handshake. It then tells the Headers that
        /// it will not accept a Cached response. It then adds Authorization to the Header, using the 
        /// _authorizationHeader string variable from this class. It sets the Verison of the Request to '1.0'. 
        /// The function then lets the requests accept images and json files as viable values for the 
        /// HttpReaderValueCollection. It then returns the Base (HttpClientHandler) Function with the passed 
        /// arguments of request and cancellationToken.
        /// 
        /// Preconditions: request must not be null, nor cancellationToken.
        /// Postconditions: Adds multiple new headers and Accept headers to the passed request, before running 
        /// the base HttpClientHandler function. 
        /// </summary>
        /// <param name="request">Passed HttpRequestMessage that will be modified in this function before being
        /// sent to the base function to Send the Async</param>
        /// <param name="cancellationToken">Passed CancellationToken that will be sent to the base function.</param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("User-Agent", "TwitterImageViewer/1.0.0.0");
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
