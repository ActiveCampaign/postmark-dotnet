﻿using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkInboundRuleTriggerList
    {
        public int TotalCount { get; set; }

        public IEnumerable<PostmarkInboundRuleTriggerInfo> InboundRules { get; set; }
    }
}
