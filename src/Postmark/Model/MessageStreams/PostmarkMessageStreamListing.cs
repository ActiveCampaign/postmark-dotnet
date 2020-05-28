using System.Collections.Generic;

namespace Postmark.Model.MessageStreams
{
    /// <summary>
    /// List of message streams
    /// </summary>
    public class PostmarkMessageStreamListing
    {
        public IEnumerable<PostmarkMessageStream> MessageStreams { get; set; }

        /// <summary>
        /// Count of total message streams
        /// </summary>
        public int TotalCount { get; set; }
    }
}
