using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace _4600Project
{
    /// <summary>
    /// The following json properties are used to access the url entity of Twitter
    /// </summary>
    public class UrlEntity
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Returns the Url in a string format
        /// </summary>
        /// <returns>Url entity</returns>
        public override string ToString()
        {
            return Url;
        }

    }
}
