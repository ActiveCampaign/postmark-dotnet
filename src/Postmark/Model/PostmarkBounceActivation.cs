namespace PostmarkDotNet.Model
{
    /// <summary>
    /// Represents the results of a request to activate a <see cref="PostmarkBounce" />.
    /// </summary>
    public class PostmarkBounceActivation
    {
        /// <summary>
        /// The server message accompanying the activation request.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// The activated bounce.
        /// </summary>
        /// <value>The activated bounce.</value>
        public PostmarkBounce Bounce { get; set; }
    }
}
