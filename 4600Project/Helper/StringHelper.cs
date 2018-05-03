using System.Text;

namespace _4600Project
{   
    public class StringHelper
    {
        private const string _AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";


        /// <summary>
        /// Cleans a string to be used in a url and appends the correct characters for encoding
        /// Used in the TwitterHttpClient
        /// 
        /// Precondition: compares what is contained in the allowed chars
        /// Postcondition: appends what is needed for the result based on the precondition
        /// </summary>  
        /// <param name="str">passind in string to be url encoded</param>
        /// <returns>the result in a string format</returns>
        public static string UrlEncode(string str)
        {
            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (_AllowedChars.Contains(c.ToString()))
                {
                    result.Append(c);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)c));
                }
            }
            return result.ToString();
        }
    }
}