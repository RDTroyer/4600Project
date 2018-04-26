using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{

    public class GetImagesPlease
    {
        const int _MaxTweetsToRetrieve = 50;
        private List<TwitterCredentials> _twitterCredsList;
        public TwitterHttpClient _twitterHttpClient;
        public GetImagesPlease(List<TwitterCredentials> TwitCredList)
        {
            _twitterCredsList = TwitCredList;
            _twitterHttpClient = new TwitterHttpClient(_twitterCredsList.First());

        }
        public void CreateTweetModelList(List<UserModel> userModels)
        {
            foreach (UserModel userModel in userModels)
            {
                try
                {
                    List<TweetEntity> tweetList =
                    _twitterHttpClient.GetUserTweetList(userModel.UserId, _MaxTweetsToRetrieve, true);
                    //userModel.TweetRetweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    //userModel.TweetModelListWrapper.TweetModelList = userModel.TweetRetweetModelList.ToList();
                    userModel.TweetModelListWrapper.TweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    foreach(TweetModel tweet in userModel.TweetModelListWrapper.TweetModelList)
                    {
                        if (!tweet.TweetImageUrlNotEmpty)
                        {
                            userModel.TweetModelListWrapper.TweetModelList.Remove(tweet);
                        }

                    }


                }
                catch (Exception exception)
                {
                    Console.WriteLine($"CreatTweetModelList => {exception.Message}");
                }
            }
        }
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
