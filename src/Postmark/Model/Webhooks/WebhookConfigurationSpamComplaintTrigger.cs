namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for SpamComplaint webhooks
    /// </summary>
    public class WebhookConfigurationSpamComplaintTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by SpamComplaint events
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Specifies whether or not the full content of the spam complaint is included in webhook POST.
        /// </summary>
        public bool IncludeContent { get; set; }
    }
}