using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        ///   The friendly name for this bounce.
        /// </summary>
        /// <value>The friendly name of the bounce</value>
        public string Name { get; set; }
    }
}
