using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    public class TwitterHttpClient
    {
        private TwitterCredentials _twitterCreds;

        /// <summary>
        /// Constructor for TwitterHttpClient, sets _twitterCreds to the passed TwitterCredentials
        /// value.
        /// 
        /// Preconditions: twitterCreds cannot be null.
        /// Postconditions: _twitterCreds is now equal to the passed TwitterCredential value.
        /// </summary>
        /// <param name="twitterCreds">Passed TwitterCredentials value.</param>
        public TwitterHttpClient(TwitterCredentials twitterCreds)
        {
            _twitterCreds = twitterCreds;
        }

        /// <summary>
        /// This method retrieves the authenicated user by creating a query for Twitter.
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <returns></returns>
        public UserEntity GetAuthenticatedUser()
        {
            TwitterQuery twitterQuery = TwitterQuery.Create(TwitterConstants.AuthUserUrl);
            twitterQuery.AddParameter("skip_status", "true");
            twitterQuery.AddParameter("include_entities", "true");
            string result = ExecuteQuery(twitterQuery);
            UserEntity user = JsonHelper.DeserializeToClass<UserEntity>(result);
            return user;
        }


        /// <summary>
        /// This method retrieves the user's friends' list based on the id of the user.
        /// 
        /// Precondition: checks if the id lengths are greater than 0
        /// Postcondition: friendList is assigned to a call which has friendIds as an argument.
        /// </summary>
        /// <param name="userId">passed userId value</param>
        /// <returns>either the friendList or a brand new UserEntity List</returns>
        public List<UserEntity> GetFriends(long userId)
        {
            List<UserEntity> friendList = null;
            long[] friendIds = GetFriendIds(userId);
            if (friendIds.Length > 0)
            {
                friendList = GetFriendsFromIds(friendIds);
            }
            return friendList ?? new List<UserEntity>();
        }


        /// <summary>
        /// The following method retrieves the user's tweet list by adding parameters to the query for Twitter access.
        /// 
        /// Precondition:none
        /// Postcondition: none
        /// </summary>
        /// <param name="userId">passed userId value</param>
        /// <param name="count">passed in count value</param>
        /// <param name="includeRetweet">boolean on choosing to include a retweet</param>
        /// <returns>tweetList or a brand new TweetEntity List</returns>
        public List<TweetEntity> GetUserTweetList(long userId, int count, bool includeRetweet = false)
        {
            var twitterQuery = TwitterQuery.Create(TwitterConstants.UserTweetsUrl);
            twitterQuery.AddParameter("user_id", userId);
            twitterQuery.AddParameter("include_rts", includeRetweet);
            twitterQuery.AddParameter("exclude_replies", false);
            twitterQuery.AddParameter("contributor_details", false);
            twitterQuery.AddParameter("count", count);
            twitterQuery.AddParameter("trim_user", false);
            twitterQuery.AddParameter("include_entities", true);
            twitterQuery.AddParameter("tweet_mode", "extended");

            string result = ExecuteQuery(twitterQuery);
            var tweetList = JsonHelper.DeserializeToClass<List<TweetEntity>>(result);
            return tweetList ?? new List<TweetEntity>();
        }

        /// <summary>
        /// This method is used to retrieve the friend ids from the user by adding the required friend parameters to the query
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <param name="userId">passed userId value</param>
        /// <returns>returns the resultIds or a new long[0]</returns>
        private long[] GetFriendIds(long userId)
        {
            var twitterQuery = TwitterQuery.Create(TwitterConstants.FriendIdsUrl);
            twitterQuery.AddParameter("user_id", userId);
            twitterQuery.AddParameter("count", TwitterConstants.MaxFriendsToRetrive);
            string result = ExecuteQuery(twitterQuery);
            var resultIds = JsonHelper.DeserializeToClass<ResultIds>(result);
            return resultIds.Ids ?? new long[0];
        }

        /// <summary>
        /// This method gets the required information for the user friends from the friend ids.
        /// 
        /// Precondition: checks if friends are null
        /// Postcondition: add the range of friends to the friendList
        /// </summary>
        /// <param name="friendIds">passed in friendIds</param>
        /// <returns>friendList</returns>
        private List<UserEntity> GetFriendsFromIds(long[] friendIds)
        {
            // Twitter allows only MaxFriendsToLookupPerCall per query, so make multiple calls
            var friendList = new List<UserEntity>();
            for (int index = 0; index < friendIds.Length; index += TwitterConstants.MaxFriendsToLookupPerCall)
            {
                var twitterQuery = TwitterQuery.Create(TwitterConstants.UsersDataUrl);
                var friendIdsToLookup = friendIds.Skip(index).Take(TwitterConstants.MaxFriendsToLookupPerCall).ToArray();
                string userIdsParam = GenerateIdsParameter(friendIdsToLookup);
                twitterQuery.AddParameter("user_id", userIdsParam);
                string queryResult = ExecuteQuery(twitterQuery);
                var friends = JsonHelper.DeserializeToClass<List<UserEntity>>(queryResult);
                if (friends == null)
                {
                    break;
                }
                friendList.AddRange(friends);
            }
            return friendList;
        }

        /// <summary>
        /// Creates the Ids parameter through string formatting
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <param name="ids">passed in ids</param>
        /// <returns>results in a Tostring format</returns>
        private string GenerateIdsParameter(long[] ids)
        {
            var result = new StringBuilder();
            for (int i = 0; i < ids.Length - 1; ++i)
            {
                result.Append(string.Format("{0}%2C", ids[i]));
            }
            result.Append(ids[ids.Length - 1]);
            return result.ToString();
        }

        /// <summary>
        /// This method executes the query by calling an Http response,
        /// seeing if it went in successfully, and returns the result.
        /// 
        /// Precondition: Checks if the httpResponseMessage is successful and if the stream is not null.
        /// Postcondition: Reads the response and makes the query result
        /// </summary>
        /// <param name="twitterQuery">passed in query for Twitter</param>
        /// <returns>results of the query, queryResult</returns>
        private string ExecuteQuery(TwitterQuery twitterQuery)
        {

            string queryResult = null;
            HttpResponseMessage httpResponseMessage = null;

            try
            {
                httpResponseMessage = GetHttpResponseAsync(twitterQuery).Result;
                var stream = httpResponseMessage.Content.ReadAsStreamAsync().Result;
                if (httpResponseMessage.IsSuccessStatusCode && stream != null)
                {
                    var responseReader = new StreamReader(stream);
                    queryResult = responseReader.ReadLine();
                }
            }
            catch (Exception)
            {
                if (httpResponseMessage != null)
                {
                    httpResponseMessage.Dispose();
                }
            }

            return queryResult;
        }

        /// <summary>
        /// This method retrieves the Http response through async as it accesses the web.
        /// The method also calls GenerateAuthroizationHeader for the Http request.
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <param name="twitterQuery">passed in query for Twitter</param>
        /// <returns>the http response</returns>
        public async Task<HttpResponseMessage> GetHttpResponseAsync(TwitterQuery twitterQuery)
        {            
            string authorizationHeader = GenerateAuthorizationHeader(twitterQuery);
            using (var httpClient = new HttpClient(new TwitterHttpClientHandler(authorizationHeader)))
            {
                return await httpClient
                                .SendAsync(new HttpRequestMessage(HttpMethod.Get, twitterQuery.QueryUrl))
                                .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method deals with the authorization portion of the query by generating signature parameters
        /// and the requirements to have an OAuthRequest.
        /// 
        /// Precondition: Checks if the length of the header is greater than 6
        /// and if the length of the formatted url parameter is greater than 0
        /// Postcondition: append the required format for the header and appends a special character for the formatted url parameter
        /// </summary>
        /// <param name="twitterQuery">passed in twitterQuery value</param>
        /// <returns>returns all athotization information in the header </returns>
        private string GenerateAuthorizationHeader(TwitterQuery twitterQuery)
        {
            var signatureParameters = new List<KeyValuePair<string, string>>();
            foreach (var queryParameter in twitterQuery.QueryParameterList)
            {
                signatureParameters.Add(new KeyValuePair<string, string>(queryParameter.Key, queryParameter.Value));
            }

            var uri = new Uri(twitterQuery.QueryUrl);
            string oauthNonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);
            var dateTime = DateTime.UtcNow;
            TimeSpan ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);

            signatureParameters.AddRange(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("oauth_consumer_key", _twitterCreds.ConsumerKey),
                new KeyValuePair<string, string>("oauth_nonce", oauthNonce),
                new KeyValuePair<string, string>("oauth_signature_method", "HMAC-SHA1"),
                new KeyValuePair<string, string>("oauth_timestamp", oauthTimestamp),
                new KeyValuePair<string, string>("oauth_token", _twitterCreds.UserAccessToken),
                new KeyValuePair<string, string>("oauth_version", "1.0"),
            });

            StringBuilder header = new StringBuilder("OAuth ");

            // Generate OAuthRequest Parameters
            StringBuilder urlParametersFormatted = new StringBuilder();
            foreach (KeyValuePair<string, string> param in (from p in signatureParameters orderby p.Key select p))
            {
                // 1) Generate header
                if (param.Key.StartsWith("oauth_"))
                {
                    if (header.Length > 6)
                    {
                        header.Append(",");
                    }
                    header.Append(string.Format("{0}=\"{1}\"", param.Key, param.Value));
                }

                // 2) Generate data for signature to be used later
                if (urlParametersFormatted.Length > 0)
                {
                    urlParametersFormatted.Append("&");
                }
                urlParametersFormatted.Append(string.Format("{0}={1}", param.Key, param.Value));
            }

            // Generate OAuthRequest
            string url = (uri.Query == string.Empty) ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, string.Empty);
            string oAuthRequest = string.Format("{0}&{1}&{2}",
                HttpMethod.Get,
                StringHelper.UrlEncode(url),
                StringHelper.UrlEncode(urlParametersFormatted.ToString()));

            // Generate OAuthSecretKey
            string oAuthSecretkey = StringHelper.UrlEncode(_twitterCreds.ConsumerSecret) + "&" +
                                    StringHelper.UrlEncode(_twitterCreds.UserAccessSecret);

            // Create signature
            HMACSHA1Generator hmacsha1Generator = new HMACSHA1Generator();
            string signature = StringHelper.UrlEncode(Convert.ToBase64String(hmacsha1Generator.ComputeHash(oAuthRequest, oAuthSecretkey, Encoding.UTF8)));

            // Append signature to Header
            header.Append($",oauth_signature=\"{signature}\"");

            return header.ToString();
        }        
    }
}
