using System.Collections.Generic;

namespace PostmarkDotNet.Model
{

    /// <summary>
    /// 
    /// </summary>
    public class HeaderCollection : Dictionary<string, string>
    {
        public HeaderCollection() : base(0) { }

        public HeaderCollection(IDictionary<string, string> baseCollection)
            : base(baseCollection)
        {
        }

    }
}
