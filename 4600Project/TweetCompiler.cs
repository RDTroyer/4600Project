﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{

    public class TweetCompiler
    {
        //Max is actually 200, but for sake of performance, went with 50.
        const int _MaxTweetsToRetrieve = 50;
        UserEntity _loginUser;
        private List<TwitterCredentials> _twitterCredsList;
        public List<UserModel> _friendsList;
        public TwitterHttpClient _twitterHttpClient;
        public TweetCompiler(List<TwitterCredentials> TwitCredList)
        {
            _twitterCredsList = TwitCredList;
            _twitterHttpClient = new TwitterHttpClient(_twitterCredsList.First());
            _loginUser = _twitterHttpClient.GetAuthenticatedUser();
            _friendsList = new List<UserModel>();
            GetFriendsList();
        }
        private void GetFriendsList()
        {
            var friends = _twitterHttpClient.GetFriends(_loginUser.Id);
            foreach(var friend in friends)
            {
                var userModel = new UserModel(friend);
                _friendsList.Add(userModel);
            }
        }
        public void CreateTweetModelList(List<UserModel> friendsList)
        {
            foreach (UserModel friend in friendsList)
            {
                try
                {
                    List<TweetEntity> tweetList =
                    _twitterHttpClient.GetUserTweetList(friend.UserId, _MaxTweetsToRetrieve, true);
                    //userModel.TweetRetweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    //userModel.TweetModelListWrapper.TweetModelList = userModel.TweetRetweetModelList.ToList();
                    friend.TweetModelListWrapper.TweetModelList = tweetList.Select(GenerateTweetModelFrom).ToList();
                    foreach(TweetModel tweet in friend.TweetModelListWrapper.TweetModelList)
                    {
                        if (!tweet.TweetImageUrlNotEmpty)
                        {
                            friend.TweetModelListWrapper.TweetModelList.Remove(tweet);
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