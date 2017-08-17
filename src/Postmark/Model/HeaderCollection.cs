using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    /// <summary>
    /// A collection of email headers.
    /// </summary>
    public class HeaderCollection : NameValueCollection
    {
        /// <summary>
        /// Instantiate an empty header collection.
        /// </summary>
        public HeaderCollection(IDictionary<string, string> baseCollection = null) : base(baseCollection ?? new Dictionary<string,string>(0))
        {
        }

        public static implicit operator HeaderCollection(Dictionary<string, string> value){
            return new HeaderCollection(value);
        }
    }
}
