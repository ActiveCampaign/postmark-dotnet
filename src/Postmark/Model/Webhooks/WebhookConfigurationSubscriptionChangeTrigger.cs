namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for SubscriptionChange webhooks
    /// </summary>
    public class WebhookConfigurationSubscriptionChangeTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by SubscriptionChange events
        /// </summary>
        public bool Enabled { get; set; }
    }
}