namespace PostmarkDotNet
{
    /// <summary>
    ///   An optional file attachment that accompanies a <see cref = "PostmarkMessage" />.
    /// </summary>
    public class PostmarkMessageAttachment
    {
        /// <summary>
        ///   The name of this attachment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   The raw, Base64 encoded content in this attachment.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///   The content type for this attachment.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The ContentID (CID) for inline image attachments
        /// </summary>
        public string ContentId { get; set; }
    }
}