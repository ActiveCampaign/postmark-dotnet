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
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkReactivationRequestStatus>))]
        public PostmarkReactivationRequestStatus Status { get; set; }

        /// <summary>
        /// Optional message regarding the status of this request.
        /// </summary>
        public string Message { get; set; }
    }
}
