using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{
    /// <summary>
    /// The following json properties are used to access the tweet entities of Twitter
    /// which consist of urls and media.
    /// </summary>
    public class TweetEntities
    {
        [JsonProperty("urls")]
        public List<UrlEntity> UrlList { get; set; }

        [JsonProperty("media")]
        public List<MediaEntity> MediaList { get; set; }
    }
}
