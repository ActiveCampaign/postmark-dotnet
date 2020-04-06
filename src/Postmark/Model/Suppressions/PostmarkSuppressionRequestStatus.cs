namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Status for the result of applying a Suppression request.
    /// </summary>
    public enum PostmarkSuppressionRequestStatus
    {
        /// <summary>
        /// We have received the request to Suppress the recipient.
        /// </summary>
        Suppressed,

        /// <summary>
        /// Request submission has failed. Please see the result Message field for information.
        /// </summary>
        Failed
    }
}
