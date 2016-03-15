using System;

namespace PostmarkDotNet.Exceptions
{
    /// <summary>
    /// An exception thrown when the Postmark API rejects a request. See the Response property for exact information on the issue.
    /// </summary>
    public class PostmarkValidationException : Exception
    {
        public PostmarkValidationException(PostmarkResponse response)
            : base(response.Message)
        {
            Response = response;
        }

        /// <summary>
        /// The complete response returned from the Postmark API.
        /// </summary>
        public PostmarkResponse Response { get; set; }
    }
}
