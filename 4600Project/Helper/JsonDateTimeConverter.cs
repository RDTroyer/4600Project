using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _4600Project
{
    public class JsonDateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// This method is used to write the json in order to balance the date time of the json information given by the json properties.
        /// 
        /// Precondition: none
        /// Postcondition: none
        /// </summary>
        /// <param name="writer">passed in to write</param>
        /// <param name="value">passed in for the value</param>
        /// <param name="serializer">passed in to serialize the json</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        /// <summary>
        /// This method is used to read in the json in order to balance the date time of the json information given by the json properties.
        /// 
        /// Precondition: checks for the DateTime
        /// Postcondition: returns the value of the reader
        /// </summary>
        /// <param name="reader">passed in to read the date/time value</param>
        /// <param name="objectType">passed in for the type</param>
        /// <param name="existingValue">passed in for an existing value</param>
        /// <param name="serializer">passed in to serialize the json</param>
        /// <returns>the value of the reader by just the value or by parsing it to construct the format of the DateTime value</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is DateTime)
            {
                return reader.Value;
            }

            return DateTime.ParseExact(reader.Value as string, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture);
        }
    }
}
