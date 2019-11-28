using System.Collections.Generic;

namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// The webhook configuration for a specific Server and Message Stream
    /// </summary>
    public class WebhookConfiguration
    {
        /// <summary>
        /// ID of the webhook configuration.
        /// </summary>
        public long? ID { get; set; }

        /// <summary>
        /// The webhook URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// MessageStream identifier that this configuration belongs to
        /// </summary>
        public string MessageStream { get; set; }

        /// <summary>
        /// Optional Basic HTTP Authentication credentials
        /// </summary>
        public HttpAuth HttpAuth { get; set; }

        /// <summary>
        /// Optional list of custom HttpHeaders to be included
        /// </summary>
        public IEnumerable<HttpHeader> HttpHeaders { get; set; } = new List<HttpHeader>();

        /// <summary>
        /// Configuration for the webhook triggers
        /// </summary>
        public WebhookConfigurationTriggers Triggers { get; set; }
    }
}