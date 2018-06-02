using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;
using Tweetinvi.Models;
using User = Tweetinvi.User;

namespace YoavShop.ExternalFeauters
{
    public class TwitterApi
    {
        public IAuthenticatedUser TwitUser { get; set; }

        public TwitterApi()
        {
            Auth.SetUserCredentials("rU8vaV2mpDHTsYTQsTnItYM18", "IvfT0wiKSrtZWyKJIwaWe7UnuEB8eKfAi2MnUSvRv9ZmBGe4Sc",
                "1002850919506235392-zemHXViy61kXHc2yZr9LiwwOdibYSn", "1kvW578ahJo6E0kX3qCKEA5T35JyyUoYfTdZKR7E8QhgB");
            TwitUser = User.GetAuthenticatedUser();
        }

        public void Tweet(string tweetMessage)
        {
            TwitUser.PublishTweet(tweetMessage);
        }
    }
}