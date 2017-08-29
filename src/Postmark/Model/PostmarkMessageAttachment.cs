using System;
using System.IO;

namespace PostmarkDotNet
{
    /// <summary>
    ///   An optional file attachment that accompanies a <see cref = "PostmarkMessage" />.
    /// </summary>
    public class PostmarkMessageAttachment
    {

        public PostmarkMessageAttachment(byte[] content = null, string name = null, string contentType = "application/octet-stream", string contentId = null)
         {
            this.Name = name;
            this.ContentType = contentType;
            this.ContentId = contentId;
            this.Content = content != null ? Convert.ToBase64String(content) : null;
        }

        // /// <summary>
        // /// Add a file from the specified path to this message.
        // /// </summary>
        // /// <param name="message"></param>
        // /// <param name="path"></param>
        // /// <param name="contentType"></param>
        // /// <param name="contentId"></param>
        // public PostmarkMessageAttachment(string path, string contentType = null, string contentId = null)
        // {
        //     var fi = new FileInfo(path);
        //     var content = File.ReadAllBytes(path);
        //     message.AddAttachment(content, fi.Name, contentType, contentId);
        // }

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