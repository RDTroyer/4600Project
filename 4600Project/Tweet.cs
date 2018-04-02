using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _4600Project
{
    class Tweet
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("hashtag")]
        public string Hashtag { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }
    }
}
