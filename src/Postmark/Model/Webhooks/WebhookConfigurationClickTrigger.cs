namespace PostmarkDotNet.Model.Webhooks
{
    /// <summary>
    /// Settings for Click webhooks
    /// </summary>
    public class WebhookConfigurationClickTrigger
    {
        /// <summary>
        /// Specifies whether or not webhooks will be triggered by Click events
        /// </summary>
        public bool Enabled { get; set; }
    }
}