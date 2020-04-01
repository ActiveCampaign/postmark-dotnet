using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying a reactivation request.
    /// </summary>
    public class PostmarkReactivationRequestResult
    {
        /// <summary>
        /// Recipient email address requested to be reactivated.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Status of the request.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PostmarkReactivationRequestStatus Status { get; set; }

        /// <summary>
        /// Optional message regarding the status of this request.
        /// </summary>
        public string Message { get; set; }
    }
}
