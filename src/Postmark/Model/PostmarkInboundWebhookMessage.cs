using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Webhooks
{
    /// <summary>
    /// Represents the model of a Postmark Inbound message post
    /// More details http://developer.postmarkapp.com/developer-inbound-parse.html
    /// </summary>
    public class PostmarkInboundWebhookMessage
    {
        /// <summary>
        /// The Address the Inbound message is originally from
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The name from the Address the Inbound message is originally from
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The fully populated From address in Name/Email format
        /// </summary>
        public FromFull FromFull { get; set; }

        /// <summary>
        /// The Address the inbound message was sent to
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The fully populated To address in Name/Email format
        /// </summary>
        public List<ToFull> ToFull { get; set; }

        /// <summary>
        /// See also To, From, Bcc
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// See also ToFull, FromFull, BccFull
        /// </summary>
        public List<CcFull> CcFull { get; set; }

        /// <summary>
        /// See also To, From, Cc
        /// </summary>
        public string Bcc { get; set; }

        /// <summary>
        /// See also ToFull, FromFull, CcFull
        /// </summary>
        public List<BccFull> BccFull { get; set; }

        /// <summary>
        /// The ReplyTo address if available from the Inbound message
        /// </summary>
        public string ReplyTo { get; set; }

        /// <summary>
        /// The subject of the Inbound message email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The Postmark message ID X-Postmark-MessageID
        /// </summary>
        public Guid MessageID { get; set; }

        /// <summary>
        /// The original recipient the messages was addressed to
        /// </summary>
        public string OriginalRecipient { get; set; }

        /// <summary>
        /// The received time of the message by Postmark
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// The Mailbox hash, if available parsed from a message like
        /// http://developer.postmarkapp.com/developer-inbound-parse.html#mailboxhash
        /// </summary>
        public string MailboxHash { get; set; }

        /// <summary>
        /// The text part of the message
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// Teh html part of the message
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Tags, if any, included in the message
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The stripped text for replies for inbound messages
        /// </summary>
        public string StrippedTextReply { get; set; }

        /// <summary>
        /// A list of headers from the message, fully parsed
        /// </summary>
        public List<Header> Headers { get; set; }

        /// <summary>
        /// A list of Attachments from the message, fully parsed as Attachment classes
        /// </summary>
        public List<Attachment> Attachments { get; set; }
    }
}