using System.Collections.Generic;

namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// The response for listing webhook configurations
    /// </summary>
    public class WebhookConfigurationListingResponse
    {
        /// <summary>
        /// Collection of WebHookConfigurations matching the query
        /// </summary>
        public IEnumerable<WebhookConfiguration> Webhooks { get; set; }
    }
}