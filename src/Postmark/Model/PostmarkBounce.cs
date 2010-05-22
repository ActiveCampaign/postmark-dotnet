using System;

namespace PostmarkDotNet
{
    public class PostmarkBounce
    {
        public PostmarkBounceType Type { get; set; }
        public string Details { get; set; }
        public string Email { get; set; }
        public DateTime BouncedAt { get; set; }
        public bool DumpAvailable { get; set; }
        public bool Inactive { get; set; }
        public bool CanActivate { get; set; }
        public string ID { get; set; }
    }

    public class PostmarkBounceDump
    {
        public string Body { get; set; }
    }
}