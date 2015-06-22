using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int TemplateId { get; set; }
    }
}
