using System.Collections.Generic;

namespace NetStandad16.Model
{
    public class NameValuePair{
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class NameValueCollection: List<NameValuePair>, ICollection<NameValuePair>
    {
        
    }
}