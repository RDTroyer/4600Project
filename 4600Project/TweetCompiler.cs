using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _4600Project
{

    public class TweetCompiler : BaseModel
    {
        //Max is actually 200, but for sake of performance, went with 50.
        const int _MaxTweetsToRetrieve = 50;
        UserEntity _loginUser;
        public DataTable dataTable;
        private List<TwitterCredentials> _twitterCredsList;
        public List<UserModel> _friendsList;
        public TwitterHttpClient _twitterHttpClient;

        public TweetModelListWrapper Wrapper { get; private set; }
       
        /// <summary>
        /// This constructor initializes the compiler fields and call for the friendsList.
        /// 
        /// Precondition: none
        /// Postcondtion: none
        /// </summary>
        /// <param name="TwitCredList">Passed in twitter credential list values</param>
        public TweetCompiler(List<TwitterCredentials> TwitCredList)
        {
            _twitterCredsList = TwitCredList;
            _twitterHttpClient = new TwitterHttpClient(_twitterCredsList.First());
            _loginUser = _twitterHttpClient.GetAuthenticatedUser();
            _friendsList = new List<UserModel>();
            GetFriendsList();
        }

        /// <summary>
        /// This method gets the friend information from the twitter client and adds them to a friendsList as a userModel
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        private void GetFriendsList()
        {
            var friends = _twitterHttpClient.GetFriends(_loginUser.Id);
            foreach(var friend in friends)
            {
                var userModel = new UserModel(friend);
                _friendsList.Add(userModel);
            }
        }

        /// <summary>
        /// The following method is used to create the tweet model list for viewing on on the xaml
        /// by assigning the friend infromation to each friend and wraps every friend to the wrapper's TweetModelList
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <param name="friendsList">passed in friendslist values</param>
        public void CreateTweetModelList(List<UserModel> friendsList)
        {

            Wrapper = new TweetModelListWrapper();
            foreach (UserModel friend in friendsList)
            {
                try
                {
                    List<TweetModel> tweetModelList = new List<TweetModel>();
                    List<TweetEntity> tweetList =
                    _twitterHttpClient.GetUserTweetList(friend.UserId, _MaxTweetsToRetrieve, true);
                    //friend.TweetRetweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    tweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    Console.WriteLine("HERE " + friend.UserName + " " + tweetModelList.Count);
                    Wrapper.TweetModelList.AddRange(tweetModelList);
                    /*if(Wrapper.TweetModelList != null)
                    {
                        MessageBox.Show($"{Wrapper.TweetModelList.Count}");
                    }*/

                    break;
               }

                catch (Exception exception)
                {
                    Console.WriteLine($"CreatTweetModelList => {exception.Message}");
                }
            }
        }

        /// <summary>
        /// The following method is used to properly generate the TweetModel for the compiler
        /// by formatting the text and the date/time of the tweet.
        /// 
        /// Precondition: check if the index for the full tweet text is 0 or greater than 0,
        /// if the span for the total hours is 0 or less than 24
        /// 
        /// Postcondition: Chooses the right formatting needed based on the precondtions
        /// </summary>
        /// <param name="tweet">passed in tweet information from TweetEntity</param>
        /// <returns> a new tweet model</returns>
        private TweetModel GenerateTweetModelFrom(TweetEntity tweet)
        {

            // Split text and embed url
            string fullText, embedUrl;
            int index = tweet.FullText.IndexOf("http");
            if (index == 0)
            {
                fullText = string.Empty;
                embedUrl = tweet.FullText.Split(' ', '\n')?[0];
            }
            else if (index > 0)
            {
                fullText = tweet.FullText.Substring(0, index);
                embedUrl = tweet.FullText.Substring(index).Split(' ', '\n')?[0];
            }
            else
            {
                fullText = tweet.FullText;
                embedUrl = string.Empty;
            }
         
            // Determine tweet date / time
            string tweetDateTime;
            TimeSpan span = DateTime.Now.Subtract(tweet.CreatedAt);
            if ((int)span.TotalHours == 0)
            {
                tweetDateTime = ((int)span.TotalMinutes > 0) ?
                                     $"{(int)span.TotalMinutes}m" : $"{(int)span.TotalSeconds}s";
            }
            else if (span.TotalHours < 24)
            {
                int hours = (int)(span.TotalMinutes > 0 ? span.TotalHours + 1 : span.TotalHours);
                tweetDateTime = $"{hours}h";
            }
            else
            {
                tweetDateTime = string.Format("{0:t}   ", tweet.CreatedAt) +
                                        string.Format("{0:MMM d}", tweet.CreatedAt);
            }
         
            return new TweetModel
            {
                TweetId = tweet.Id,
                TweetUrl = $"https://twitter.com/{tweet.CreatedBy.ScreenName}/status/{tweet.Id}",
                TweetFullText = fullText,
                IsRetweet = tweet.RetweetedTweet != null,
                TweetEmbedUrl = embedUrl,
                TweetImageUrl = tweet.Entities?.MediaList?[0].MediaUrl,
                TweetDateTime = tweetDateTime,
            };
        }
    }
}
