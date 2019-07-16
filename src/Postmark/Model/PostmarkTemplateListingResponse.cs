using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        public string Alias { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TemplateType TemplateType { get; set; }

        public string LayoutTemplate { get; set; }
    }
}
