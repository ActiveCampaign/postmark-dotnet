using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkTaggedTriggerList
    {
        public int TotalCount { get; set; }

        public IEnumerable<PostmarkTaggedTriggerInfo> Tags { get; set; }
    }
}
