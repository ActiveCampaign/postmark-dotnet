using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        [JsonConverter(typeof(StringEnumConverter))]
        public PostmarkSuppressionRequestStatus Status { get; set; }

        /// <summary>
        /// Optional message regarding the status of this request.
        /// </summary>
        public string Message { get; set; }
    }
}
