using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Webhooks
{
    /// <summary>
    /// Representation of the payload of the delivery webhook
    /// - http://developer.postmarkapp.com/developer-delivery-webhook.html
    /// </summary>
    public class PostmarkDeliveryWebhookMessage
    {
        /// <summary>
        /// The associated server's ID.
        /// </summary>
        [JsonProperty("ServerId")]
        public int ServerID { get; set; }

        /// <summary>
        /// The associated message's ID.
        /// </summary>
        public Guid MessageID { get; set; }

        /// <summary>
        /// The email address of the recipient.
        /// </summary>
        public string Recipient { get; set; }

        /// <summary>
        /// The delivery tag that was used when the message was sent.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The timestamp of when email was delivered.
        /// </summary>
        public DateTimeOffset DeliveredAt { get; set; }

        /// <summary>
        /// The response line received from the destination email server.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// The metadata for this message.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
