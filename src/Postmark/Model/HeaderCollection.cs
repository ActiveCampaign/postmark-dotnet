using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class MailHeader {
        public MailHeader(string name = null, string value = null)
        {
            Name = name;
            Value = value;
        }
        
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class HeaderCollection: List<MailHeader>, ICollection<MailHeader>
    {
        public HeaderCollection() : base() { }
        
        public HeaderCollection(IDictionary<string, string> nameValues = null)
        {
            if (nameValues != null)
            {
                foreach (var f in (nameValues)){
                    this.Add(new MailHeader(f.Key, f.Value));
                }
            }
        }

        public HeaderCollection(IEnumerable<MailHeader> nameValues = null)
        {
            if (nameValues != null)
            {
                this.AddRange(nameValues);
            }
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