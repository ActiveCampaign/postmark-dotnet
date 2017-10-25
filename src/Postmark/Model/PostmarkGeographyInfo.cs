using System;

namespace PostmarkDotNet.Model
{
    [Obsolete("This class is deprecated, because it is used for both opens, and clicks now. Use 'PostmarkGeographyInfo' instead.")]
    public class PostmarkGeographyOpenInfo: PostmarkGeographyInfo {

    }

    public class PostmarkGeographyInfo
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string IP { get; set; }
        public string RegionISOCode { get; set; }
        public string CountryISOCode { get; set; }
        public string Coords { get; set; }
    }
}
