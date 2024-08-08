namespace Postmark.Model.MessageStreams
{
    public class PostmarkSubscriptionManagementConfiguration
    {
        /// <summary>
        /// The unsubscribe management option used for the Stream. Broadcast Message Streams require unsubscribe management, Postmark is default. For Inbound and Transactional Streams default is none.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<UnsubscribeHandlingType>))]
        public UnsubscribeHandlingType UnsubscribeHandlingType { get; set; }
    }
}