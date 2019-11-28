namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for Bounce webhooks
    /// </summary>
    public class WebhookConfigurationBounceTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by Bounce events
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies whether or not the full content of the email bounce is included in webhook POST.
        /// </summary>
        public bool IncludeContent { get; set; }
    }
}