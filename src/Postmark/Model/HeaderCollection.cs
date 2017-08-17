using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class HeaderCollection : NameValueCollection
    {
        public HeaderCollection() : base() { }

        public HeaderCollection(IDictionary<string, string> baseCollection) : base(baseCollection)
        {
        }

        public static implicit operator HeaderCollection(Dictionary<string, string> value){
            return new HeaderCollection(value);
        }
    }
}
