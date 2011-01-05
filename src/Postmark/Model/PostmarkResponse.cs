using System;

namespace PostmarkDotNet
{
    /// <summary>
    ///   A response from the Postmark API after a request sent with <see cref = "PostmarkClient" />.
    /// </summary>
    public class PostmarkResponse
    {
        /// <summary>
        ///   The status outcome of the response.
        /// </summary>
        public PostmarkStatus Status { get; set; }

        /// <summary>
        ///   The message from the API.  
        ///   In the event of an error, this message may contain helpful text.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///   The time the request was received by Postmark.
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        ///   The recipient of the submitted request.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        ///   The error code returned from Postmark.
        ///   This does not map to HTTP status codes.
        /// </summary>
        public int ErrorCode { get; set; }
    }
}