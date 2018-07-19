using System.Collections.Generic;

namespace PostmarkDotNet.Webhooks
{

    /// <summary>
    /// Representation of the payload of the bounce webhook
    /// </summary>
    public class PostmarkBounceWebhookMessage : PostmarkBounce
    {
        /// <summary>
        ///   The int based type code for this bounce.
        /// </summary>
        /// <value>The type code</value>
        public int TypeCode { get; set; }

        /// <summary>
        /// The metadata for the bounced message.
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }
    }
}
