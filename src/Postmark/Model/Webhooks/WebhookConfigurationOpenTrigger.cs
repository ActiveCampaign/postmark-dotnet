namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for Open webhooks
    /// </summary>
    public class WebhookConfigurationOpenTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by Open events
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If enabled, open webhook will only POST on first open.
        /// </summary>
        public bool PostFirstOpenOnly { get; set; }
    }
}