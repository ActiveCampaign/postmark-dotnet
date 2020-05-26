using System;

namespace Postmark.Model.MessageStreams
{
    /// <summary>
    /// Confirmation of archiving a message stream.
    /// </summary>
    public class PostmarkMessageStreamArchivalConfirmation
    {
        /// <summary>
        /// Identifier of the message stream that was archived.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Id of the server where this stream was archived.
        /// </summary>
        public int ServerID { get; set; }

        /// <summary>
        /// Expected date when this archived message stream will be removed, alongside associated content.
        /// The stream can be unarchived up until this date.
        /// </summary>
        public DateTime ExpectedPurgeDate { get; set; }
    }
}
