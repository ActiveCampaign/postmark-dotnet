using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class To
    {
        public string Email { get; set; }
        public object Name { get; set; }
    }

    public class Cc
    {
        public string Email { get; set; }
        public object Name { get; set; }
    }

    public class Bcc
    {
        public string Email { get; set; }
        public object Name { get; set; }
    }

    public class Message
    {
        public string Tag { get; set; }
        public string MessageID { get; set; }
        public List<To> To { get; set; }
        public List<Cc> Cc { get; set; }
        public List<Bcc> Bcc { get; set; }
        public List<string> Recipients { get; set; }
        public string ReceivedAt { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public List<string> Attachments { get; set; }
    }

    public class OutboundMessageDetail : Message
    {
        public string TextBody { get; set; }
        public string HtmlBody { get; set; }
        public string Body { get; set; }
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
