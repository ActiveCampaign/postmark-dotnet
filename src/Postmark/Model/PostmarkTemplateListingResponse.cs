using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkTemplateListingResponse
    {
        public int TotalCount { get; set; }

        public IEnumerable<BasicTemplateInformation> Templates { get; set; }
    }

    public class BasicTemplateInformation
    {
        public bool IsActive { get; set; }
        public String Name { get; set; }
        public long TemplateId { get; set; }
    }
}
