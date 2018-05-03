using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace _4600Project
{
    public static class JsonHelper
    {
        /// <summary>
        /// Used to serialize an object as a json string.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string SerializeObjectAsJsonString(object objectToSerialize)
        {
            string jsonString = JsonConvert.SerializeObject(objectToSerialize, Newtonsoft.Json.Formatting.Indented
                                    , new JsonSerializerSettings
                                            {
                                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                            }
                                    );
            return jsonString;
        }

        public static void SaveAsJsonToFile(object objectToSerialize, string filePath)
        {
            string jsonString = SerializeObjectAsJsonString(objectToSerialize);
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                File.WriteAllText(filePath, jsonString);
            }
        }

        /// <summary>
        /// This method is used to deserialize json to a class
        /// Used for the TwitterHttpClient
        /// 
        /// Precondition: checks if the json string is null
        /// Postcondition: either return a default or the class object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">the passed in json string</param>
        /// <returns>default or the class object</returns>
        public static T DeserializeToClass<T>(string jsonString)
        {

            if (string.IsNullOrWhiteSpace(jsonString))
                return default(T);

            T @class = JsonConvert.DeserializeObject<T>(jsonString);
            return @class;
        }

        /// <summary>
        /// This method is used to deserialize json from a file using the file path
        /// Used for the TwitterHttpClient
        /// 
        /// Precondition: checks if the json string is null
        /// Postcondition: either return a default or the class object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonfilePath">passed in json file path</param>
        /// <returns>default or the class object</returns>
        public static T DeserializeFromFile<T>(string jsonfilePath)
        {
            string jsonString = File.ReadAllText(jsonfilePath);
            if (string.IsNullOrWhiteSpace(jsonString))
                return default(T);

            T @class = JsonConvert.DeserializeObject<T>(jsonString);
            return @class;
        }
    }
}
