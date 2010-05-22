using System.Collections.Generic;

namespace PostmarkDotNet
{
    public class PostmarkBounces
    {
        public int TotalCount { get; set; }
        public List<PostmarkBounce> Bounces{ get; set; }
    }
}