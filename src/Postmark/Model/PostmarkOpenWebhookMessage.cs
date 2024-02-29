using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Webhooks
{
    /// <summary>
    /// Representation of the payload of the open tracking webhook
    /// </summary>
    public class PostmarkOpenWebhookMessage : PostmarkOpen
    {
        /// <summary>
        ///   The time the open was received by the Postmark servers.
        /// </summary>
        /// <value>The time the open was received</value>
        public DateTime ReceivedAt { get; set; }

        /// <summary>
        /// The tags users add to emails
        /// </summary>
        /// <value>The specific tag string</value>
        public string Tag { get; set; }

        /// <summary>
        /// The email address of the recipient who opened the email.
        /// </summary>
        /// <value>Email address of the recipient</value>
        public string Recipient { get; set; }

        /// <summary>
        /// The metadata for the opened message.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }

}
