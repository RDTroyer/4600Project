using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{ 
    public class TwitterConstants
    {
        public const string AuthUserUrl = "https://api.twitter.com/1.1/account/verify_credentials.json";
        public const string UserTweetsUrl = "https://api.twitter.com/1.1/statuses/user_timeline.json";
        public const string FriendIdsUrl = "https://api.twitter.com/1.1/friends/ids.json";
        public const string UsersDataUrl = "https://api.twitter.com/1.1/users/lookup.json";
        public const string TonUrl = "https://ton.twitter.com";
        public const int MaxFriendsToRetrive = 250;
        public const int MaxFriendsToLookupPerCall = 100; // Twitter API's limit
    }
}
