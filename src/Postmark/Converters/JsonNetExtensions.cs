using Newtonsoft.Json;

namespace PostmarkDotNet.Converters
{
    internal static class JsonNetExtensions
    {
        /// <summary>
        /// Attempt to deserialize a string to the specified type.
        /// </summary>
        /// <typeparam name="T">The type that we should attempt to deserialize to.</typeparam>
        /// <param name="json">The source JSON to attempt to deserialize.</param>
        /// <param name="result">The object that is created, if successful.</param>
        /// <returns>True if deserialization is successful, false otherwise.</returns>
        internal static bool TryDeserializeObject<T>(string json, out T result)
        {
            var retval = false;
            result = default(T);
            try
            {
                var settings = new JsonSerializerSettings()
               {
                   MissingMemberHandling = MissingMemberHandling.Ignore,
                   NullValueHandling = NullValueHandling.Include,
                   DefaultValueHandling = DefaultValueHandling.Include
               };

                settings.Converters.Add(new UnicodeJsonStringConverter());
                result = JsonConvert.DeserializeObject<T>(json);
                retval = true;
            }
            catch
            {
                //swallow this exception;   
            }
            return retval;
        }

    }
}
