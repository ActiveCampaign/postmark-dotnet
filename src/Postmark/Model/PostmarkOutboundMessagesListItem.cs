using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PostmarkDotNet.Model
{
    public class To
    {
        public string Email { get; set; }
        public object Name { get; set; }
    }

    public class Message
    {
        public string Tag { get; set; }
        public string MessageID { get; set; }
        public List<To> To { get; set; }
        public List<object> Cc { get; set; }
        public List<object> Bcc { get; set; }
        public List<string> Recipients { get; set; }
        public string ReceivedAt { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public List<object> Attachments { get; set; }
    }

    public class PostmarkOutboundMessageListItem
    {
        public int TotalCount { get; set; }
        public List<Message> Messages { get; set; }
    }
}
