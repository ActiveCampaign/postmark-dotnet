namespace PostmarkDotNet
{
    /// <summary>
    ///   A list of possible status outcomes associated with a <see cref = "PostmarkResponse" />.
    /// </summary>
    public enum PostmarkStatus
    {
        /// <summary>
        ///   The response is unknown to this version of the Postmark.NET API.
        /// </summary>
        Unknown = -1,
        /// <summary>
        ///   The request was successful.
        /// </summary>
        Success = 0,
        /// <summary>
        ///   The request was unsuccessful because of a user error.
        /// </summary>
        UserError,
        /// <summary>
        ///   The request was unsuccessful because of a server error.
        /// </summary>
        ServerError
    }
}