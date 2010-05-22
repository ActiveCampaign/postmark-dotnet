using System.Collections.Generic;

namespace PostmarkDotNet
{
    // {"InactiveMails":0,"Bounces":[{"TypeCode":0,"Name":"All","Count":0}]}

    public class PostmarkDeliveryStats
    {
        public int InactiveMails { get; set; }
        public List<PostmarkBounceSummary> Bounces { get; set; }
    }
}
