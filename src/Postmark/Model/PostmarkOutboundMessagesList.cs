using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class Recipient
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class Message
    {
        public string Tag { get; set; }
        public string MessageID { get; set; }
        public List<Recipient> To { get; set; }
        public List<Recipient> Cc { get; set; }
        public List<Recipient> Bcc { get; set; }
        public List<string> Recipients { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public List<string> Attachments { get; set; }
        public string Status { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
    }

    public class OutboundMessageDetail : Message
    {
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string Body { get; set; }
        public IEnumerable<MessageEvent> MessageEvents { get; set; } 
    }

    public class MessageEvent
    {
        public string Recipient { get; set; }
        public string Type { get; set; }
        public DateTime ReceivedAt { get; set; }
        public Dictionary<string, string> Details { get; set; }
    }

    public class MessageDump
    {
        public string Body { get; set; }
    }

    public class PostmarkOutboundMessageList
    {
        public int TotalCount { get; set; }
        public List<Message> Messages { get; set; }
    }
}
