using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace _4600Project
{ 
    public class TwitterQuery
    {

        /// <summary>
        /// Constructor for TwitterQuery. Sets QueryParameterList as a new empty list of KeyValuePair objects.
        /// 
        /// Preconditions: None
        /// Postconditions: QueryParameterList of object is now set to an empty list. 
        /// </summary>
        private TwitterQuery()
        {
            QueryParameterList = new List<KeyValuePair<string, string>>();
        }


        /// <summary>
        /// Creates a new Twitter Query Object, storing the passed 'url' to be stored
        /// in the QueryUrl Property
        /// 
        /// Preconditions: None
        /// Postconditions: Creates a new TwitterQuery Object, fills the QueryUrl with 
        /// the passed url, and returns that TwitterQuery Object.
        /// </summary>
        /// <param name="url">The url of a given Twitter User (The Authorized User,
        /// the Auth User's friend, etc.</param>
        /// <returns>Returns the new TwitterQuery Object.</returns>
        public static TwitterQuery Create(string url)
        {
            return new TwitterQuery
            {
                QueryUrl = url
            };
        }

        /// <summary>
        /// A string property that contains the Url of a specific user to be queried.
        /// </summary>
        public string QueryUrl { get; private set; }

        /// <summary>
        /// A List of KeyValuePair objects.
        /// </summary>
        public List<KeyValuePair<string, string>> QueryParameterList { get; }
        
        /// <summary>
        /// AddParameter is an overloaded version of itself which allows the passed 'value'
        /// to be an object rather than exclusively a string.
        /// 
        /// Preconditions: None.
        /// Postconditions: Calls AddParameter(string, string) with the passed 'value' object
        /// converted to a string.
        /// </summary>
        /// <param name="key">Key of a KeyValuePair to be added.</param>
        /// <param name="value">Value of a KeyValuePair to be added.</param>
        public void AddParameter(string key, object value)
        {
            AddParameter(key, value?.ToString());
        }

        /// <summary>
        /// This version of AddParameter takes the passed key and value, checks to see if QueryUrl is
        /// Empty, and if it is, adds the key and value to it, with a '?' added to the front. If
        /// QueryUrl is not empty, the passed key and value get added to the existing QueryUrl with a '&' added to 
        /// the front. The passed key and value are then used as arguements to make a new KeyValuePair,
        /// which will be added to QueryParameterList.
        /// 
        /// Preconditions: value must be a string.
        /// Postconditions: key and value will be added to the QueryUrl and added as a KeyValuePiar to
        /// the QueryParameterList.
        /// </summary>
        /// <param name="key">The Key of a KeyValuePair to be added.</param>
        /// <param name="value">The Value of a KeyValuePair to be added.</param>
        public void AddParameter(string key, string value)
        {
            if (QueryParameterList.Count == 0)
            {
                QueryUrl += $"?{key}={value}";
            }
            else
            {
                QueryUrl += $"&{key}={value}";
            }

            // Build key / value paramater list for signing
            QueryParameterList.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
