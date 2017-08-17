using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class NameValuePair {
        public NameValuePair(string name = null, string value = null)
        {
            Name = name;
            Value = value;
        }
        
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class NameValueCollection: List<NameValuePair>, ICollection<NameValuePair>
    {
        public NameValueCollection() : base() { }
        public NameValueCollection(IDictionary<string, string> nameValues = null)
        {
            
        }
        public NameValueCollection(IEnumerable<NameValuePair> nameValues = null)
        {

        }

        /// <summary>
        /// Get the names associated with this collection. This property does not cache
        /// its results, so be aware that iterating over it multiple times will iterate over
        /// all elements of the collection each time.
        /// </summary>
        public IEnumerable<string> Keys
        {
            get
            {
                var keys = new List<string>(this.Capacity);
                foreach(var f in this)
                {
                    if (!keys.Contains(f.Name))
                    {
                        keys.Add(f.Name);
                    }
                }
                return keys;
            }
        }
    }
}