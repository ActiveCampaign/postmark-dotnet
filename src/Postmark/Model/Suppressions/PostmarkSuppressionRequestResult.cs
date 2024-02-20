using PostmarkDotNet;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying a Suppression request.
    /// </summary>
    public class PostmarkSuppressionRequestResult
    {
        /// <summary>
        /// Recipient email address requested to be suppressed.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Status of the request.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkBounceType>))]
        public PostmarkSuppressionRequestStatus Status { get; set; }

        /// <summary>
        /// Optional message regarding the status of this request.
        /// </summary>
        public string Message { get; set; }
    }
}
