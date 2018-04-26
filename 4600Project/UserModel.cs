using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    public class UserModel
    {
        public UserModel(UserEntity user)
        {
            UserId = user.Id;
            UserName = user.Name;
            ScreenName = user.ScreenName;
            ProfileImageUrl = user.ProfileImageUrl;
            TweetRetweetModelList = new List<TweetModel>();
            TweetModelListWrapper = new TweetModelListWrapper();
            RetweetModelListWrapper = new TweetModelListWrapper();

        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public string ProfileImageUrl { get; set; }
        public List<TweetModel> TweetRetweetModelList { get; set; }
        public TweetModelListWrapper TweetModelListWrapper { get; set; }
        public TweetModelListWrapper RetweetModelListWrapper { get; set; }
       
       
      
    }
}
