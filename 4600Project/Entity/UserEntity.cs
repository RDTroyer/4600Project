using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _4600Project
{
    /// <summary>
    /// The following json properties are used to access the user entity information of Twitter
    /// </summary>
    public class UserEntity
    {
        [JsonProperty("id")]
        public long Id { get; set; }
      
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
           
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
               
        /// <summary>
        /// Returns the name, screen name, and id information from user entity into a string format
        /// </summary>
        /// <returns>Name, ScreenName, Id</returns>
        public override string ToString()
        {
            return $"{Name} | {ScreenName} | {Id}";
        }
    }
}
