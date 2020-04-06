namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Status for the result of reactivating a recipient.
    /// </summary>
    public enum PostmarkReactivationRequestStatus
    {
        /// <summary>
        /// We have received the request to reactivate the recipient.
        /// </summary>
        Deleted,

        /// <summary>
        /// Request submission has failed. Please see the result Message field for information.
        /// </summary>
        Failed
    }
}
