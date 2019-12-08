namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for Delivery webhooks
    /// </summary>
    public class WebhookConfigurationDeliveryTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by Delivery events
        /// </summary>
        public bool Enabled { get; set; }
    }
}